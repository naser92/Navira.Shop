/** @typedef {import("./types.js").GridFilter} GridFilter */

/**
 * Clamp a number into an inclusive range.
 * @param {number} value
 * @param {number} min
 * @param {number} max
 */
export function clamp(value, min, max) {
  return Math.min(max, Math.max(min, value));
}

/**
 * Debounce a callback. Returns a stable function plus a cancel handle.
 * @template {(...args: unknown[]) => void} F
 * @param {F} fn
 * @param {number} ms
 */
export function debounce(fn, ms) {
  let timer = null;
  const debounced = (...args) => {
    if (timer) clearTimeout(timer);
    timer = setTimeout(() => {
      timer = null;
      fn(...args);
    }, ms);
  };
  debounced.cancel = () => {
    if (timer) clearTimeout(timer);
    timer = null;
  };
  return debounced;
}

/**
 * Normalize a committed search string: trim and collapse to null when empty.
 * @param {string|null|undefined} value
 * @returns {string|null}
 */
export function normalizeSearch(value) {
  if (typeof value !== "string") return null;
  const trimmed = value.trim();
  return trimmed.length > 0 ? trimmed : null;
}

const isPrimitive = (v) =>
  v === null || ["string", "number", "boolean"].includes(typeof v);

/**
 * Validate that a filter is complete enough to send to the server.
 * UX-level validation only — the backend remains authoritative.
 * @param {GridFilter} filter
 * @returns {boolean}
 */
export function isCompleteFilter(filter) {
  if (!filter || typeof filter.field !== "string" || filter.field.length === 0)
    return false;
  if (typeof filter.operator !== "string" || filter.operator.length === 0)
    return false;

  switch (filter.operator) {
    case "isNull":
    case "isNotNull":
      return true;
    case "between":
      return filter.value !== undefined && filter.value !== null && filter.value !== ""
        ? filter.value2 !== undefined && filter.value2 !== null && filter.value2 !== ""
        : false;
    case "in":
    case "notIn":
      return (
        Array.isArray(filter.value) &&
        filter.value.length > 0 &&
        filter.value.every(isPrimitive)
      );
    default:
      return (
        filter.value !== undefined &&
        filter.value !== null &&
        filter.value !== "" &&
        (isPrimitive(filter.value) || Array.isArray(filter.value))
      );
  }
}

/**
 * Strip undefined values from a filter so incomplete pieces are never sent.
 * @param {GridFilter} filter
 * @returns {GridFilter}
 */
export function sanitizeFilter(filter) {
  const out = { field: filter.field, operator: filter.operator };
  if (filter.value !== undefined) out.value = filter.value;
  if (filter.value2 !== undefined) out.value2 = filter.value2;
  return out;
}

/**
 * Resolve a stable row identity without assuming an `id` field.
 * Order: explicit getRowId → primitive `id`/`_id` property → null.
 * Never falls back to the array index (not a stable identity).
 *
 * @template TData
 * @param {TData} row
 * @param {((row: TData) => string)|undefined} getRowId
 * @returns {string|null}
 */
export function resolveRowId(row, getRowId) {
  if (getRowId) {
    const id = getRowId(row);
    return typeof id === "string" && id.length > 0 ? id : null;
  }
  if (row && typeof row === "object") {
    const record = /** @type {Record<string, unknown>} */ (row);
    for (const key of ["id", "_id"]) {
      const v = record[key];
      if (typeof v === "string" && v.length > 0) return v;
      if (typeof v === "number" && Number.isFinite(v)) return String(v);
    }
  }
  return null;
}

/**
 * Compute the inclusive 1-based display range for a page.
 * @param {number} page
 * @param {number} pageSize
 * @param {number} totalCount
 * @returns {{from: number, to: number}}
 */
export function paginationRange(page, pageSize, totalCount) {
  if (totalCount <= 0) return { from: 0, to: 0 };
  const from = (page - 1) * pageSize + 1;
  const to = Math.min(page * pageSize, totalCount);
  return { from, to };
}

/**
 * Locale-aware number formatting (respects the project's fa locale).
 * @param {number} value
 * @param {string} [locale]
 */
export function formatNumber(value, locale = "fa-IR") {
  try {
    return new Intl.NumberFormat(locale).format(value);
  } catch {
    return String(value);
  }
}
