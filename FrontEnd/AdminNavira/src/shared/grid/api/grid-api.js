/**
 * Grid API adapter.
 *
 * Reuses the project's existing `apiFetch` client — Bearer token handling,
 * token refresh, centralized 401/403 behavior all stay in one place.
 * This adapter only handles the grid transport contract and response
 * envelope unwrapping. Tokens are never read, stored, or logged here.
 */

/** @typedef {import("../core/types.js").GridRequest} GridRequest */
/** @typedef {import("../core/types.js").GridResponse<unknown>} GridResponse */

import { apiFetch } from "@/lib/api/clientApi.js";

/** Error code used when a response cannot be adapted to GridResponse. */
export const GRID_RESPONSE_INVALID = "GRID_RESPONSE_INVALID";

/**
 * Unwrap the project's response envelope into a GridResponse.
 * Supports `{ data: GridResponse, error, message }` and bare GridResponse.
 *
 * @param {unknown} payload
 * @returns {GridResponse}
 */
export function unwrapGridResponse(payload) {
  const candidate =
    payload &&
    typeof payload === "object" &&
    "data" in payload &&
    payload.data &&
    typeof payload.data === "object"
      ? payload.data
      : payload;

  if (!candidate || typeof candidate !== "object") {
    throw gridResponseError("Grid response payload is not an object");
  }
  const r = /** @type {Record<string, unknown>} */ (candidate);
  if (!Array.isArray(r.items)) {
    throw gridResponseError("Grid response is missing an items array");
  }

  const totalCount = toNonNegativeInt(r.totalCount, r.items.length);
  const pageSize = toNonNegativeInt(r.pageSize, r.items.length || 1);
  const page = toNonNegativeInt(r.page, 1);
  const totalPages = toNonNegativeInt(
    r.totalPages,
    Math.max(1, Math.ceil(totalCount / Math.max(1, pageSize)))
  );

  return {
    items: r.items,
    page,
    pageSize,
    totalCount,
    totalPages,
    hasPrevious:
      typeof r.hasPrevious === "boolean" ? r.hasPrevious : page > 1,
    hasNext:
      typeof r.hasNext === "boolean" ? r.hasNext : page < totalPages,
    columns: Array.isArray(r.columns) ? r.columns : undefined,
  };
}

/** @param {unknown} value @param {number} fallback */
function toNonNegativeInt(value, fallback) {
  const n = Number(value);
  if (Number.isInteger(n) && n >= 0) return n;
  return Math.max(0, Math.trunc(fallback));
}

/** @param {string} message */
function gridResponseError(message) {
  const error = new Error(message);
  error.code = GRID_RESPONSE_INVALID;
  return error;
}

/**
 * Fetch grid data from a server endpoint.
 *
 * @template TData
 * @param {string} endpoint
 * @param {GridRequest} request
 * @param {AbortSignal} [signal]
 * @returns {Promise<import("../core/types.js").GridResponse<TData>>}
 */
export async function fetchGrid(endpoint, request, signal) {
  const payload = await apiFetch(endpoint, {
    method: "POST",
    body: JSON.stringify(request),
    ...(signal ? { signal } : {}),
  });
  return /** @type {import("../core/types.js").GridResponse<TData>} */ (
    unwrapGridResponse(payload)
  );
}
