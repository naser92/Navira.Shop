/**
 * Strict validation of persisted grid preferences.
 *
 * Zod is not part of this project's stack, so validation uses explicit
 * type guards. Invalid or corrupt data safely yields null and never
 * throws — corrupt Local Storage must never break the grid.
 */

/** @typedef {import("./types.js").GridPreferences} GridPreferences */
/** @typedef {import("./types.js").GridSort} GridSort */
/** @typedef {import("./types.js").GridFilter} GridFilter */

import { isFilterOperator } from "./operators.js";
import { clampWidth } from "./column-utils.js";

export const PREFERENCES_KEY_PREFIX = "grid-preferences:";

/** @param {string} gridKey */
export function preferencesStorageKey(gridKey) {
  return `${PREFERENCES_KEY_PREFIX}${gridKey}`;
}

/** @param {unknown} v @returns {v is GridSort} */
function isSort(v) {
  if (!v || typeof v !== "object") return false;
  const s = /** @type {Record<string, unknown>} */ (v);
  return (
    typeof s.field === "string" &&
    (s.direction === "asc" || s.direction === "desc")
  );
}

/** @param {unknown} v @returns {v is GridFilter} */
function isFilter(v) {
  if (!v || typeof v !== "object") return false;
  const f = /** @type {Record<string, unknown>} */ (v);
  return typeof f.field === "string" && isFilterOperator(f.operator);
}

/**
 * Parse and validate raw JSON into GridPreferences.
 * Unknown/invalid fields are ignored; wholly invalid input returns null.
 *
 * @param {unknown} raw - already JSON.parsed value
 * @returns {Partial<GridPreferences>|null}
 */
export function parsePreferences(raw) {
  if (!raw || typeof raw !== "object" || Array.isArray(raw)) return null;
  const p = /** @type {Record<string, unknown>} */ (raw);

  /** @type {Partial<GridPreferences>} */
  const out = {};
  let any = false;

  if (typeof p.pageSize === "number" && Number.isInteger(p.pageSize) && p.pageSize >= 1 && p.pageSize <= 500) {
    out.pageSize = p.pageSize;
    any = true;
  }
  if (Array.isArray(p.visibleColumns) && p.visibleColumns.every((v) => typeof v === "string")) {
    out.visibleColumns = p.visibleColumns;
    any = true;
  }
  if (Array.isArray(p.columnOrder) && p.columnOrder.every((v) => typeof v === "string")) {
    out.columnOrder = p.columnOrder;
    any = true;
  }
  if (p.columnWidths && typeof p.columnWidths === "object" && !Array.isArray(p.columnWidths)) {
    /** @type {Record<string, number>} */
    const widths = {};
    for (const [field, w] of Object.entries(p.columnWidths)) {
      const clamped = clampWidth(w);
      if (clamped !== null) widths[field] = clamped;
    }
    out.columnWidths = widths;
    any = true;
  }
  if (Array.isArray(p.sorts) && p.sorts.every(isSort)) {
    out.sorts = p.sorts;
    any = true;
  }
  if (Array.isArray(p.filters) && p.filters.every(isFilter)) {
    out.filters = p.filters;
    any = true;
  }

  return any ? out : null;
}

/**
 * SSR-safe read of persisted preferences. Returns null on the server,
 * on corrupt JSON, or on invalid shape.
 *
 * @param {string} gridKey
 * @returns {Partial<GridPreferences>|null}
 */
export function readPreferences(gridKey) {
  if (typeof window === "undefined" || !window.localStorage) return null;
  try {
    const raw = window.localStorage.getItem(preferencesStorageKey(gridKey));
    if (!raw) return null;
    return parsePreferences(JSON.parse(raw));
  } catch {
    return null;
  }
}

/**
 * SSR-safe write of preferences.
 * @param {string} gridKey
 * @param {GridPreferences} preferences
 */
export function writePreferences(gridKey, preferences) {
  if (typeof window === "undefined" || !window.localStorage) return;
  try {
    window.localStorage.setItem(
      preferencesStorageKey(gridKey),
      JSON.stringify(preferences)
    );
  } catch {
    // Quota/serialization failures must never break the grid.
  }
}

/** @param {string} gridKey */
export function clearPreferences(gridKey) {
  if (typeof window === "undefined" || !window.localStorage) return;
  try {
    window.localStorage.removeItem(preferencesStorageKey(gridKey));
  } catch {
    /* ignore */
  }
}
