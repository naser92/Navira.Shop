/** @typedef {import("./types.js").FilterOperatorValue} FilterOperatorValue */
/** @typedef {import("./types.js").GridColumnMetadata} GridColumnMetadata */

export const FilterOperator = Object.freeze({
  Eq: "eq",
  NotEq: "neq",
  Contains: "contains",
  StartsWith: "startsWith",
  EndsWith: "endsWith",
  GreaterThan: "gt",
  GreaterThanOrEqual: "gte",
  LessThan: "lt",
  LessThanOrEqual: "lte",
  Between: "between",
  In: "in",
  NotIn: "notIn",
  IsNull: "isNull",
  IsNotNull: "isNotNull",
});

/** @type {ReadonlySet<FilterOperatorValue>} */
const ALL_OPERATORS = new Set(Object.values(FilterOperator));

/** Operators that never carry a value. */
export const NO_VALUE_OPERATORS = new Set([
  FilterOperator.IsNull,
  FilterOperator.IsNotNull,
]);

/** Operators requiring both value and value2. */
export const TWO_VALUE_OPERATORS = new Set([FilterOperator.Between]);

/** Operators whose value must be an array. */
export const ARRAY_OPERATORS = new Set([
  FilterOperator.In,
  FilterOperator.NotIn,
]);

/**
 * Framework-safe default operators per column type.
 * @type {Record<string, FilterOperatorValue[]>}
 */
export const DEFAULT_OPERATORS_BY_TYPE = Object.freeze({
  string: [
    FilterOperator.Contains,
    FilterOperator.Eq,
    FilterOperator.NotEq,
    FilterOperator.StartsWith,
    FilterOperator.EndsWith,
    FilterOperator.IsNull,
    FilterOperator.IsNotNull,
  ],
  number: [
    FilterOperator.Eq,
    FilterOperator.NotEq,
    FilterOperator.GreaterThan,
    FilterOperator.GreaterThanOrEqual,
    FilterOperator.LessThan,
    FilterOperator.LessThanOrEqual,
    FilterOperator.Between,
    FilterOperator.IsNull,
    FilterOperator.IsNotNull,
  ],
  date: [
    FilterOperator.Eq,
    FilterOperator.GreaterThanOrEqual,
    FilterOperator.LessThanOrEqual,
    FilterOperator.Between,
    FilterOperator.IsNull,
    FilterOperator.IsNotNull,
  ],
  datetime: [
    FilterOperator.Eq,
    FilterOperator.GreaterThanOrEqual,
    FilterOperator.LessThanOrEqual,
    FilterOperator.Between,
    FilterOperator.IsNull,
    FilterOperator.IsNotNull,
  ],
  boolean: [FilterOperator.Eq],
  enum: [
    FilterOperator.Eq,
    FilterOperator.NotEq,
    FilterOperator.In,
    FilterOperator.NotIn,
  ],
  select: [
    FilterOperator.Eq,
    FilterOperator.NotEq,
    FilterOperator.In,
    FilterOperator.NotIn,
  ],
});

/** @param {unknown} op @returns {op is FilterOperatorValue} */
export function isFilterOperator(op) {
  return (
    typeof op === "string" && ALL_OPERATORS.has(/** @type {FilterOperatorValue} */ (op))
  );
}

/**
 * Resolve the allowed operators for a column:
 * metadata.filter.operators intersected with framework-safe type defaults.
 * Metadata may narrow the list but never widen it beyond type-safe operators.
 *
 * @param {GridColumnMetadata} column
 * @returns {FilterOperatorValue[]}
 */
export function operatorsForColumn(column) {
  const typeDefaults =
    DEFAULT_OPERATORS_BY_TYPE[column.type] ?? DEFAULT_OPERATORS_BY_TYPE.string;
  const meta = column.filter?.operators?.filter(isFilterOperator);
  if (!meta || meta.length === 0) return typeDefaults;
  const allowed = meta.filter((op) => typeDefaults.includes(op));
  return allowed.length > 0 ? allowed : typeDefaults;
}
