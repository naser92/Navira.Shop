/** @typedef {import("./types.js").GridRequest} GridRequest */

import { normalizeRequest } from "./normalization.js";

/**
 * Stable, deterministic query key for a grid request.
 * Shape: ["grid", gridKey, endpoint, normalizedRequest]
 *
 * The request is normalized before keying so equivalent states always map
 * to the same key regardless of object identity.
 *
 * @param {string} gridKey
 * @param {string} endpoint
 * @param {GridRequest} request
 * @returns {readonly [string, string, string, GridRequest]}
 */
export function gridQueryKey(gridKey, endpoint, request) {
  return ["grid", gridKey, endpoint, normalizeRequest(request)];
}

/**
 * Prefix key matching every request variant of a grid — used for
 * invalidation (e.g., after mutations that change list data).
 * @param {string} gridKey
 */
export function gridInvalidationKey(gridKey) {
  return ["grid", gridKey];
}
