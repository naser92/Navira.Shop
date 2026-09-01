/**
 * Deterministic, URL-safe serialization of grid state.
 *
 * Format:
 *   ?page=2&pageSize=50&search=iphone
 *   &sort=createdDate.desc,price.asc
 *   &filters=<base64url(JSON)>
 *
 * Filters use base64url-encoded compact JSON so arbitrary operators and
 * values round-trip losslessly while remaining deterministic. Never put
 * sensitive values into URL-synced filters.
 */

/** @typedef {import("./types.js").GridFilter} GridFilter */
/** @typedef {import("./types.js").GridSort} GridSort */
/** @typedef {{page?: number, pageSize?: number, search?: string|null, sorts?: GridSort[], filters?: GridFilter[]}} GridUrlState */

import { normalizeFilter, normalizeSort } from "./normalization.js";
import { normalizeSearch } from "./utils.js";

/**
 * @param {GridSort[]} sorts
 * @returns {string} e.g. "createdDate.desc,price.asc"
 */
export function serializeSorts(sorts) {
  return (sorts ?? [])
    .map(normalizeSort)
    .filter(Boolean)
    .map((s) => `${s.field}.${s.direction}`)
    .join(",");
}

/**
 * @param {string|null|undefined} param
 * @returns {GridSort[]}
 */
export function parseSorts(param) {
  if (!param || typeof param !== "string") return [];
  return param
    .split(",")
    .map((part) => {
      const idx = part.lastIndexOf(".");
      if (idx <= 0) return null;
      return normalizeSort({
        field: part.slice(0, idx),
        direction: /** @type {"asc"|"desc"} */ (part.slice(idx + 1)),
      });
    })
    .filter(/** @returns {s is GridSort} */ (s) => s !== null);
}

/** @param {string} str */
function toBase64Url(str) {
  // Encode UTF-8 safely without btoa's latin1 restriction.
  const bytes = new TextEncoder().encode(str);
  let binary = "";
  for (const byte of bytes) binary += String.fromCharCode(byte);
  return btoa(binary).replace(/\+/g, "-").replace(/\//g, "_").replace(/=+$/, "");
}

/** @param {string} encoded */
function fromBase64Url(encoded) {
  const base64 = encoded.replace(/-/g, "+").replace(/_/g, "/");
  const binary = atob(base64);
  const bytes = Uint8Array.from(binary, (ch) => ch.charCodeAt(0));
  return new TextDecoder().decode(bytes);
}

/**
 * @param {GridFilter[]} filters
 * @returns {string|null} null when there is nothing to serialize
 */
export function serializeFilters(filters) {
  const clean = (filters ?? [])
    .map(normalizeFilter)
    .filter(/** @returns {f is GridFilter} */ (f) => f !== null);
  if (clean.length === 0) return null;
  try {
    return toBase64Url(JSON.stringify(clean));
  } catch {
    return null;
  }
}

/**
 * @param {string|null|undefined} param
 * @returns {GridFilter[]} malformed input safely yields []
 */
export function parseFilters(param) {
  if (!param || typeof param !== "string") return [];
  try {
    const parsed = JSON.parse(fromBase64Url(param));
    if (!Array.isArray(parsed)) return [];
    return parsed
      .map(normalizeFilter)
      .filter(/** @returns {f is GridFilter} */ (f) => f !== null);
  } catch {
    return [];
  }
}

/**
 * Serialize grid state into search params, omitting defaults and
 * preserving unrelated existing params.
 *
 * @param {GridUrlState} state
 * @param {{defaultPageSize?: number, existing?: URLSearchParams}} [options]
 * @returns {URLSearchParams}
 */
export function toSearchParams(state, options = {}) {
  const params = new URLSearchParams(options.existing?.toString() ?? "");
  const { defaultPageSize = 20 } = options;

  setOrDelete(params, "page", state.page && state.page > 1 ? String(state.page) : null);
  setOrDelete(
    params,
    "pageSize",
    state.pageSize && state.pageSize !== defaultPageSize
      ? String(state.pageSize)
      : null
  );
  setOrDelete(params, "search", normalizeSearch(state.search));

  const sortParam = serializeSorts(state.sorts ?? []);
  setOrDelete(params, "sort", sortParam.length > 0 ? sortParam : null);

  setOrDelete(params, "filters", serializeFilters(state.filters ?? []));
  return params;
}

/** @param {URLSearchParams} params @param {string} key @param {string|null} value */
function setOrDelete(params, key, value) {
  if (value === null || value === undefined || value === "") params.delete(key);
  else params.set(key, value);
}

/**
 * Parse validated grid state from search params. Malformed values are
 * ignored safely and never throw.
 *
 * @param {URLSearchParams|{get: (k: string) => string|null}} params
 * @returns {GridUrlState}
 */
export function fromSearchParams(params) {
  const page = parsePositiveInt(params.get("page"));
  const pageSize = parsePositiveInt(params.get("pageSize"));
  const search = normalizeSearch(params.get("search"));
  const sorts = parseSorts(params.get("sort"));
  const filters = parseFilters(params.get("filters"));

  /** @type {GridUrlState} */
  const state = {};
  if (page !== null) state.page = page;
  if (pageSize !== null) state.pageSize = pageSize;
  if (search !== null) state.search = search;
  if (sorts.length > 0) state.sorts = sorts;
  if (filters.length > 0) state.filters = filters;
  return state;
}

/** @param {string|null} raw */
function parsePositiveInt(raw) {
  if (!raw) return null;
  const n = Number(raw);
  if (!Number.isInteger(n) || n < 1 || n > 100000) return null;
  return n;
}
