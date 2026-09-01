/** @typedef {import("./types.js").GridColumnMetadata} GridColumnMetadata */
/** @typedef {import("./types.js").GridPreferences} GridPreferences */
/** @typedef {import("./types.js").GridColumnOverride<unknown>} GridColumnOverride */
/** @typedef {Record<string, GridColumnOverride|undefined>} GridColumnOverrides */

export const MIN_COLUMN_WIDTH = 60;
export const MAX_COLUMN_WIDTH = 600;
export const DEFAULT_COLUMN_WIDTH = 160;

import { clamp } from "./utils.js";

/**
 * Clamp a column width into the safe range.
 * @param {unknown} width
 * @returns {number|null}
 */
export function clampWidth(width) {
  if (typeof width !== "number" || !Number.isFinite(width)) return null;
  return clamp(Math.round(width), MIN_COLUMN_WIDTH, MAX_COLUMN_WIDTH);
}

/**
 * Reconcile a persisted column order with current metadata:
 * - keep persisted order for fields that still exist
 * - append new backend fields in their natural position (end)
 * - drop obsolete fields
 *
 * @param {string[]} persistedOrder
 * @param {GridColumnMetadata[]} metadata
 * @returns {string[]}
 */
export function reconcileColumnOrder(persistedOrder, metadata) {
  const fields = metadata.map((c) => c.field);
  const fieldSet = new Set(fields);
  const seen = new Set();

  const ordered = [];
  for (const field of persistedOrder ?? []) {
    if (typeof field === "string" && fieldSet.has(field) && !seen.has(field)) {
      ordered.push(field);
      seen.add(field);
    }
  }
  for (const field of fields) {
    if (!seen.has(field)) {
      ordered.push(field);
      seen.add(field);
    }
  }
  return ordered;
}

/**
 * Reconcile persisted visible columns with metadata: drop obsolete fields,
 * include new fields that metadata marks visible (or doesn't hide).
 *
 * @param {string[]} persistedVisible
 * @param {GridColumnMetadata[]} metadata
 * @returns {string[]}
 */
export function reconcileVisibleColumns(persistedVisible, metadata) {
  const fieldSet = new Set(metadata.map((c) => c.field));
  const persistedSet = new Set(
    (persistedVisible ?? []).filter(
      (f) => typeof f === "string" && fieldSet.has(f)
    )
  );
  const visible = [];
  for (const column of metadata) {
    if (persistedSet.has(column.field)) {
      visible.push(column.field);
    } else if (!persistedVisible || persistedVisible.length === 0) {
      // No persisted state: fall back to metadata default.
      if (column.visible !== false) visible.push(column.field);
    }
    // Fields present in metadata but absent from persisted state after a
    // prior save stay hidden (user explicitly hid them or they are new).
  }
  return visible;
}

/**
 * Final column resolution order:
 *   backend metadata → framework defaults → persisted preferences →
 *   frontend overrides.
 *
 * Frontend overrides can never enable sorting/filtering that the backend
 * metadata disables — capabilities are backend-owned.
 *
 * @param {GridColumnMetadata[]} metadata
 * @param {Partial<GridPreferences>|null} preferences
 * @param {GridColumnOverrides} [overrides]
 * @returns {{columns: GridColumnMetadata[], order: string[], visible: Set<string>, widths: Record<string, number>, overrides: GridColumnOverrides}}
 */
export function resolveColumns(metadata, preferences, overrides = {}) {
  const safeMetadata = Array.isArray(metadata) ? metadata : [];

  const order = reconcileColumnOrder(preferences?.columnOrder ?? [], safeMetadata);
  const visibleList = reconcileVisibleColumns(
    preferences?.visibleColumns ?? [],
    safeMetadata
  );

  // Frontend explicit visibility override (can only hide/show, not add caps).
  const visible = new Set(visibleList);
  for (const [field, override] of Object.entries(overrides ?? {})) {
    if (!override) continue;
    if (override.visible === true && safeMetadata.some((c) => c.field === field))
      visible.add(field);
    if (override.visible === false) visible.delete(field);
  }

  /** @type {Record<string, number>} */
  const widths = {};
  for (const column of safeMetadata) {
    const persisted = clampWidth(preferences?.columnWidths?.[column.field]);
    const metaWidth = clampWidth(column.width);
    const w = persisted ?? metaWidth;
    if (w !== null) widths[column.field] = w;
  }

  return { columns: safeMetadata, order, visible, widths, overrides: overrides ?? {} };
}
