/** @typedef {import("./types.js").GridRequest} GridRequest */
/** @typedef {import("./types.js").GridFilter} GridFilter */
/** @typedef {import("./types.js").GridSort} GridSort */

import { isCompleteFilter, normalizeSearch, sanitizeFilter } from "./utils.js";
import { isFilterOperator } from "./operators.js";

/**
 * Normalize a filter for stable hashing and transport.
 * - drops incomplete filters (UX-level; backend validates again)
 * - strips undefined values
 * - sorts `in`/`notIn` arrays deterministically
 * @param {GridFilter} filter
 * @returns {GridFilter|null}
 */
export function normalizeFilter(filter) {
  if (!filter || typeof filter.field !== "string") return null;
  if (!isFilterOperator(filter.operator)) return null;
  if (!isCompleteFilter(filter)) return null;

  const clean = sanitizeFilter(filter);
  if (
    (clean.operator === "in" || clean.operator === "notIn") &&
    Array.isArray(clean.value)
  ) {
    clean.value = [...clean.value].sort(comparePrimitives);
  }
  return clean;
}

/** @param {unknown} a @param {unknown} b */
function comparePrimitives(a, b) {
  const sa = typeof a === "number" ? a : String(a);
  const sb = typeof b === "number" ? b : String(b);
  if (sa < sb) return -1;
  if (sa > sb) return 1;
  return 0;
}

/**
 * Normalize a sort entry; invalid entries are dropped.
 * @param {GridSort} sort
 * @returns {GridSort|null}
 */
export function normalizeSort(sort) {
  if (!sort || typeof sort.field !== "string" || sort.field.length === 0)
    return null;
  if (sort.direction !== "asc" && sort.direction !== "desc") return null;
  return { field: sort.field, direction: sort.direction };
}

/**
 * Produce a canonical, reference-stable request object used both for
 * transport and for the TanStack Query key. Property order is fixed.
 *
 * @param {GridRequest} request
 * @returns {GridRequest}
 */
export function normalizeRequest(request) {
  const filters = (request.filters ?? [])
    .map(normalizeFilter)
    .filter(/** @returns {f is GridFilter} */ (f) => f !== null);
  const sorts = (request.sorts ?? [])
    .map(normalizeSort)
    .filter(/** @returns {s is GridSort} */ (s) => s !== null);

  return {
    page: Math.max(1, Math.trunc(request.page) || 1),
    pageSize: Math.max(1, Math.trunc(request.pageSize) || 1),
    search: normalizeSearch(request.search),
    filters,
    sorts,
  };
}

/**
 * Deep structural equality for normalized requests (arrays and plain
 * objects of primitives). Used to avoid redundant state updates.
 * @param {unknown} a
 * @param {unknown} b
 */
export function deepEqual(a, b) {
  if (a === b) return true;
  if (a === null || b === null) return false;
  if (typeof a !== "object" || typeof b !== "object") return false;
  if (Array.isArray(a) !== Array.isArray(b)) return false;

  if (Array.isArray(a) && Array.isArray(b)) {
    if (a.length !== b.length) return false;
    return a.every((item, i) => deepEqual(item, b[i]));
  }

  const aKeys = Object.keys(/** @type {Record<string, unknown>} */ (a));
  const bKeys = Object.keys(/** @type {Record<string, unknown>} */ (b));
  if (aKeys.length !== bKeys.length) return false;
  return aKeys.every((key) =>
    deepEqual(
      /** @type {Record<string, unknown>} */ (a)[key],
      /** @type {Record<string, unknown>} */ (b)[key]
    )
  );
}
