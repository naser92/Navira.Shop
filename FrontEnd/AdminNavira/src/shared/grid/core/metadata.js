/** @typedef {import("./types.js").GridColumnMetadata} GridColumnMetadata */

import { isFilterOperator } from "./operators.js";

const COLUMN_TYPES = new Set([
  "string",
  "number",
  "boolean",
  "date",
  "datetime",
  "enum",
  "select",
]);

/**
 * @param {unknown} value
 * @returns {value is GridColumnMetadata}
 */
export function isValidColumnMetadata(value) {
  if (!value || typeof value !== "object") return false;
  const c = /** @type {Record<string, unknown>} */ (value);
  if (typeof c.field !== "string" || c.field.length === 0) return false;
  if (typeof c.title !== "string") return false;
  if (!COLUMN_TYPES.has(/** @type {string} */ (c.type))) return false;
  if (c.filter !== undefined) {
    if (typeof c.filter !== "object" || c.filter === null) return false;
    const f = /** @type {Record<string, unknown>} */ (c.filter);
    if (f.operators !== undefined) {
      if (!Array.isArray(f.operators) || !f.operators.every(isFilterOperator))
        return false;
    }
    if (f.options !== undefined) {
      if (!Array.isArray(f.options)) return false;
      const ok = f.options.every(
        (o) =>
          o &&
          typeof o === "object" &&
          typeof (/** @type {Record<string, unknown>} */ (o).label) ===
            "string" &&
          ["string", "number", "boolean"].includes(
            typeof /** @type {Record<string, unknown>} */ (o).value
          )
      );
      if (!ok) return false;
    }
  }
  return true;
}

/**
 * Validate a metadata array; invalid entries are dropped, never fatal.
 * @param {unknown} columns
 * @returns {GridColumnMetadata[]}
 */
export function validateMetadata(columns) {
  if (!Array.isArray(columns)) return [];
  return columns.filter(isValidColumnMetadata);
}

/**
 * Metadata source resolution:
 *   1. Backend response columns (when present and valid)
 *   2. Explicitly provided frontend metadata prop
 *   3. Otherwise null → caller surfaces a developer-facing config error.
 *
 * We never infer production metadata from arbitrary runtime row values.
 *
 * @param {unknown} responseColumns
 * @param {GridColumnMetadata[]|undefined} propMetadata
 * @returns {{metadata: GridColumnMetadata[], source: "backend"|"props"|"none"}}
 */
export function resolveMetadata(responseColumns, propMetadata) {
  const fromBackend = validateMetadata(responseColumns);
  if (fromBackend.length > 0) return { metadata: fromBackend, source: "backend" };
  const fromProps = validateMetadata(propMetadata);
  if (fromProps.length > 0) return { metadata: fromProps, source: "props" };
  return { metadata: [], source: "none" };
}
