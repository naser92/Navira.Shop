/** @typedef {import("../core/types.js").GridSort} GridSort */

import { useCallback } from "react";

/**
 * Sorting interactions.
 * - Click toggles: none → asc → desc → none (single-sort) or cycles within
 *   the multi-sort list when multiSort is enabled (shift-click style via
 *   explicit "add" behaviour on header click when multiSortable).
 * - Sort priority is the array order.
 *
 * @param {Object} args
 * @param {GridSort[]} args.sorts
 * @param {(next: GridSort[]) => void} args.setSorts
 * @param {boolean} [args.multiSort]
 */
export function useGridSort({ sorts, setSorts, multiSort = false }) {
  /**
   * Toggle sorting for a field.
   * @param {string} field
   * @param {boolean} additive - when true and multiSort enabled, keep others
   */
  const toggleSort = useCallback(
    (field, additive = false) => {
      const existingIndex = sorts.findIndex((s) => s.field === field);
      const existing = existingIndex >= 0 ? sorts[existingIndex] : null;

      let nextDirection = /** @type {"asc"|"desc"|null} */ ("asc");
      if (existing?.direction === "asc") nextDirection = "desc";
      else if (existing?.direction === "desc") nextDirection = null;

      if (!multiSort || !additive) {
        setSorts(nextDirection ? [{ field, direction: nextDirection }] : []);
        return;
      }

      const next = sorts.filter((s) => s.field !== field);
      if (nextDirection) {
        // Keep existing position when cycling; otherwise append.
        if (existingIndex >= 0) next.splice(existingIndex, 0, { field, direction: nextDirection });
        else next.push({ field, direction: nextDirection });
      }
      setSorts(next);
    },
    [sorts, setSorts, multiSort]
  );

  /** @param {string} field @returns {{direction: "asc"|"desc"|null, priority: number|null}} */
  const getSortState = useCallback(
    (field) => {
      const index = sorts.findIndex((s) => s.field === field);
      if (index < 0) return { direction: null, priority: null };
      return {
        direction: sorts[index].direction,
        priority: sorts.length > 1 ? index + 1 : null,
      };
    },
    [sorts]
  );

  return { toggleSort, getSortState };
}
