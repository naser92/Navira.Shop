/**
 * Search is orchestrated inside `useGrid` (debounced input → committed
 * server-side search with page reset). This module re-exports the small
 * pure helpers so the filter/search behaviour stays independently testable.
 */
export { normalizeSearch } from "../core/utils.js";
export { DEFAULT_DEBOUNCE_MS } from "./useGrid.js";
