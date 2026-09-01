/**
 * Central grid state orchestration.
 *
 * Owns page/pageSize/search/filters/sorts, applies the documented state
 * precedence (validated URL state → persisted preferences → props), wires
 * debounced search, persists preferences, and keeps page=1 invariants on
 * search/filter/sort/page-size changes.
 */

/** @typedef {import("../core/types.js").SmartGridProps<TData>} SmartGridProps @template TData */
/** @typedef {import("../core/types.js").GridRequest} GridRequest */
/** @typedef {import("../core/types.js").GridFilter} GridFilter */
/** @typedef {import("../core/types.js").GridSort} GridSort */
/** @typedef {import("../core/types.js").GridPreferences} GridPreferences */

import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { normalizeSearch } from "../core/utils.js";
import { deepEqual, normalizeRequest } from "../core/normalization.js";
import { useGridPreferences } from "./useGridPreferences.js";
import { useGridUrlState } from "./useGridUrlState.js";

export const DEFAULT_PAGE_SIZE = 20;
export const DEFAULT_PAGE_SIZE_OPTIONS = [10, 20, 50, 100];
export const DEFAULT_DEBOUNCE_MS = 500;

/**
 * @param {Object} args
 * @param {string} args.gridKey
 * @param {number} [args.pageSize]
 * @param {boolean} [args.syncUrl]
 * @param {boolean} [args.persistPreferences]
 * @param {number} [args.debounceMs]
 */
export function useGrid({
  gridKey,
  pageSize: pageSizeProp,
  syncUrl = false,
  persistPreferences = true,
  debounceMs = DEFAULT_DEBOUNCE_MS,
}) {
  const persistEnabled = persistPreferences !== false;
  const { preferences, hydrated, save, reset } = useGridPreferences({
    gridKey,
    enabled: persistEnabled,
  });
  const { urlState, writeUrl, consumeUrlState } = useGridUrlState({
    enabled: syncUrl,
    defaultPageSize: pageSizeProp ?? DEFAULT_PAGE_SIZE,
  });

  const initialPageSize =
    (typeof pageSizeProp === "number" && pageSizeProp >= 1
      ? pageSizeProp
      : DEFAULT_PAGE_SIZE);

  // Raw state. URL/preference hydration happens in an effect after mount
  // so the first client render matches the server render (no mismatch).
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(initialPageSize);
  const [searchInput, setSearchInput] = useState("");
  const [search, setSearch] = useState(/** @type {string|null} */ (null));
  const [filters, setFilters] = useState(/** @type {GridFilter[]} */ ([]));
  const [sorts, setSorts] = useState(/** @type {GridSort[]} */ ([]));

  const initializedRef = useRef(false);

  // Initial state precedence: URL (when syncUrl) → preferences → defaults.
  useEffect(() => {
    if (initializedRef.current) return;
    if (syncUrl && urlState === null) return; // wait for URL parse
    if (!syncUrl && !hydrated) return; // wait for preference read
    initializedRef.current = true;

    const source = syncUrl && urlState ? urlState : preferences;
    if (!source) {
      if (syncUrl) consumeUrlState();
      return;
    }

    if (typeof source.page === "number") setPage(Math.max(1, source.page));
    if (typeof source.pageSize === "number" && source.pageSize >= 1)
      setPageSize(source.pageSize);
    const restoredSearch = normalizeSearch(source.search ?? null);
    if (restoredSearch) {
      setSearchInput(restoredSearch);
      setSearch(restoredSearch);
    }
    if (Array.isArray(source.filters) && source.filters.length > 0)
      setFilters(source.filters);
    if (Array.isArray(source.sorts) && source.sorts.length > 0)
      setSorts(source.sorts);
    if (syncUrl) consumeUrlState();
  }, [syncUrl, hydrated, urlState, preferences, consumeUrlState]);

  // Apply later URL changes (back/forward navigation).
  useEffect(() => {
    if (!syncUrl || !urlState || !initializedRef.current) return;
    setPage(urlState.page ?? 1);
    if (typeof urlState.pageSize === "number") setPageSize(urlState.pageSize);
    const restoredSearch = normalizeSearch(urlState.search ?? null);
    setSearchInput(restoredSearch ?? "");
    setSearch(restoredSearch);
    setFilters(urlState.filters ?? []);
    setSorts(urlState.sorts ?? []);
    consumeUrlState();
  }, [syncUrl, urlState, consumeUrlState]);

  // Debounced search commit: page resets to 1.
  const searchTimerRef = useRef(null);
  useEffect(() => () => {
    if (searchTimerRef.current) clearTimeout(searchTimerRef.current);
  }, []);

  /** @param {string} value */
  const updateSearchInput = useCallback(
    (value) => {
      setSearchInput(value);
      if (searchTimerRef.current) clearTimeout(searchTimerRef.current);
      searchTimerRef.current = setTimeout(() => {
        const committed = normalizeSearch(value);
        setSearch((prev) => {
          if (prev === committed) return prev;
          setPage(1);
          return committed;
        });
      }, debounceMs);
    },
    [debounceMs]
  );

  /** @param {GridFilter[]} next */
  const updateFilters = useCallback((next) => {
    setFilters((prev) => (deepEqual(prev, next) ? prev : next));
    setPage(1);
  }, []);

  /** @param {GridSort[]} next */
  const updateSorts = useCallback((next) => {
    setSorts((prev) => (deepEqual(prev, next) ? prev : next));
    setPage(1);
  }, []);

  /** @param {number} next */
  const updatePageSize = useCallback((next) => {
    setPageSize(next);
    setPage(1);
  }, []);

  /** @param {number} next */
  const updatePage = useCallback((next) => {
    setPage(Math.max(1, next));
  }, []);

  const clearFiltersAndSearch = useCallback(() => {
    if (searchTimerRef.current) clearTimeout(searchTimerRef.current);
    setSearchInput("");
    setSearch(null);
    setFilters([]);
    setPage(1);
  }, []);

  /** The canonical request sent to the server and used in the query key. */
  const request = useMemo(
    () =>
      normalizeRequest({ page, pageSize, search, filters, sorts }),
    [page, pageSize, search, filters, sorts]
  );

  // URL sync write-through (loop-guarded inside useGridUrlState).
  useEffect(() => {
    if (!syncUrl || !initializedRef.current) return;
    writeUrl({ page, pageSize, search, filters, sorts });
  }, [syncUrl, page, pageSize, search, filters, sorts, writeUrl]);

  /**
   * Persist user grid preferences (pageSize/sorts/filters are captured
   * here; column state is merged by the caller via `persistColumnState`).
   * @param {Partial<GridPreferences>} columnState
   */
  const persistAll = useCallback(
    (columnState) => {
      if (!persistEnabled || !initializedRef.current) return;
      save({
        pageSize,
        sorts,
        filters,
        visibleColumns: columnState.visibleColumns ?? [],
        columnOrder: columnState.columnOrder ?? [],
        columnWidths: columnState.columnWidths ?? {},
      });
    },
    [persistEnabled, save, pageSize, sorts, filters]
  );

  const resetPreferences = useCallback(() => {
    reset();
    setPage(1);
    setPageSize(initialPageSize);
    setSorts([]);
  }, [reset, initialPageSize]);

  return {
    page,
    pageSize,
    searchInput,
    search,
    filters,
    sorts,
    request,
    initializedRef,
    preferences,
    preferencesHydrated: hydrated,
    setPage: updatePage,
    setPageSize: updatePageSize,
    setSearchInput: updateSearchInput,
    setFilters: updateFilters,
    setSorts: updateSorts,
    clearFiltersAndSearch,
    persistAll,
    resetPreferences,
  };
}
