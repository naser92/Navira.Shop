/** @typedef {import("../core/serialization.js").GridUrlState} GridUrlState */

import { useCallback, useEffect, useRef, useState } from "react";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { fromSearchParams, toSearchParams } from "../core/serialization.js";
import { deepEqual } from "../core/normalization.js";

/**
 * Optional URL synchronization for grid state.
 *
 * - Parses validated URL state once on mount (and on back/forward).
 * - Pushes state via `router.replace` (no full navigation, no history spam
 *   for committed state transitions of a single logical view).
 * - Guards against update loops by comparing serialized output with the
 *   current query string before writing.
 *
 * @param {Object} args
 * @param {boolean} args.enabled
 * @param {number} args.defaultPageSize
 */
export function useGridUrlState({ enabled, defaultPageSize }) {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  /** State parsed from the URL, consumed exactly once per navigation. */
  const [urlState, setUrlState] = useState(
    /** @type {GridUrlState|null} */ (null)
  );
  const lastWrittenRef = useRef("");

  // Parse on mount and whenever the query string changes externally
  // (back/forward navigation).
  useEffect(() => {
    if (!enabled) return;
    const current = searchParams.toString();
    if (current === lastWrittenRef.current) return; // our own write
    setUrlState(fromSearchParams(searchParams));
  }, [enabled, searchParams]);

  /**
   * Push current grid state into the URL. Loop-safe: skips when the
   * serialized params already match the address bar.
   * @param {GridUrlState} state
   */
  const writeUrl = useCallback(
    (state) => {
      if (!enabled) return;
      const params = toSearchParams(state, {
        defaultPageSize,
        existing: searchParams,
      });
      const next = params.toString();
      if (next === searchParams.toString()) return;
      lastWrittenRef.current = next;
      router.replace(next ? `${pathname}?${next}` : pathname, {
        scroll: false,
      });
    },
    [enabled, defaultPageSize, pathname, router, searchParams]
  );

  /** Mark URL state as consumed after the grid has applied it. */
  const consumeUrlState = useCallback(() => setUrlState(null), []);

  return { urlState, writeUrl, consumeUrlState };
}
