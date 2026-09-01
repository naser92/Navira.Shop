/**
 * SmartGrid core contracts.
 *
 * The project is JavaScript; this module documents the framework's
 * TypeScript-style contracts via JSDoc so editors provide strong typing
 * without changing the build pipeline.
 *
 * Public generic usage:
 *   <SmartGrid gridKey="products" endpoint="/api/products/grid" />
 */

/**
 * @typedef {"eq"|"neq"|"contains"|"startsWith"|"endsWith"|"gt"|"gte"|"lt"|"lte"|"between"|"in"|"notIn"|"isNull"|"isNotNull"} FilterOperatorValue
 */

/**
 * @typedef {Object} GridFilter
 * @property {string} field
 * @property {FilterOperatorValue} operator
 * @property {unknown} [value]
 * @property {unknown} [value2]
 */

/**
 * @typedef {Object} GridSort
 * @property {string} field
 * @property {"asc"|"desc"} direction
 */

/**
 * @typedef {Object} GridRequest
 * @property {number} page - one-based page index (server contract)
 * @property {number} pageSize
 * @property {string|null} [search]
 * @property {GridFilter[]} filters
 * @property {GridSort[]} sorts
 */

/**
 * @typedef {Object} GridFilterOption
 * @property {string} label
 * @property {string|number|boolean} value
 */

/**
 * @typedef {Object} GridFilterMetadata
 * @property {"text"|"number"|"date"|"datetime"|"boolean"|"enum"|"select"} type
 * @property {FilterOperatorValue[]} [operators]
 * @property {GridFilterOption[]} [options]
 */

/**
 * @typedef {Object} GridColumnMetadata
 * @property {string} field
 * @property {string} title
 * @property {"string"|"number"|"boolean"|"date"|"datetime"|"enum"|"select"} type
 * @property {boolean} [searchable]
 * @property {boolean} [filterable]
 * @property {boolean} [sortable]
 * @property {boolean} [visible]
 * @property {number} [width]
 * @property {GridFilterMetadata} [filter]
 */

/**
 * @template TData
 * @typedef {Object} GridResponse
 * @property {TData[]} items
 * @property {number} page
 * @property {number} pageSize
 * @property {number} totalCount
 * @property {number} totalPages
 * @property {boolean} hasPrevious
 * @property {boolean} hasNext
 * @property {GridColumnMetadata[]} [columns]
 */

/**
 * @template TData
 * @typedef {Object} GridCellContext
 * @property {unknown} value
 * @property {TData} row
 * @property {number} rowIndex - index within the current page
 * @property {GridColumnMetadata} column
 */

/**
 * Strongly typed per-column frontend overrides. Keys are backend field names.
 *
 * @template TData
 * @typedef {Object} GridColumnOverride
 * @property {(ctx: GridCellContext<TData>) => import("react").ReactNode} [render]
 * @property {(column: GridColumnMetadata) => import("react").ReactNode} [header]
 * @property {"start"|"center"|"end"} [align]
 * @property {number} [minWidth]
 * @property {number} [maxWidth]
 * @property {boolean} [visible] - visibility override; cannot enable filtering/sorting
 * @property {(value: unknown, row: TData) => string} [exportValue]
 */

/**
 * @template TData
 * @typedef {Partial<Record<Extract<keyof TData, string>, GridColumnOverride<TData>>> & Record<string, GridColumnOverride<TData>>} GridColumnOverrides
 */

/**
 * @template TData
 * @typedef {Object} GridBulkActionContext
 * @property {TData[]} selectedRows - rows encountered on loaded pages
 * @property {ReadonlySet<string>} selectedIds
 */

/**
 * @template TData
 * @typedef {Object} GridBulkActionResult
 * @property {"clear"|"retain"} [selection] - default "clear" on success
 */

/**
 * @template TData
 * @typedef {Object} GridBulkAction
 * @property {string} id
 * @property {string} label
 * @property {(ctx: GridBulkActionContext<TData>) => Promise<GridBulkActionResult|void>|GridBulkActionResult|void} action
 * @property {boolean} [destructive]
 * @property {boolean} [disabled]
 * @property {string|boolean} [confirm] - confirmation message; when true a generic message is used
 */

/**
 * @typedef {Object} GridExportContext
 * @property {Omit<GridRequest, "page"|"pageSize">} request
 * @property {"csv"|"excel"} format
 * @property {string} gridKey
 * @property {string} endpoint
 */

/**
 * @typedef {Object} GridExportOptions
 * @property {boolean} enabled
 * @property {Array<"csv"|"excel">} [formats]
 * @property {(ctx: GridExportContext) => void|Promise<void>} [onExport] - consumer/server-side handler
 */

/**
 * @typedef {Object} GridQueryOptions
 * @property {number} [staleTime]
 * @property {number} [gcTime]
 * @property {boolean} [enabled]
 */

/**
 * @typedef {Object} GridPreferences
 * @property {number} pageSize
 * @property {string[]} visibleColumns
 * @property {string[]} columnOrder
 * @property {Record<string, number>} columnWidths
 * @property {GridSort[]} sorts
 * @property {GridFilter[]} filters
 */

/**
 * @typedef {Object} GridLocalization
 * @property {string} searchPlaceholder
 * @property {string} searchLabel
 * @property {string} filters
 * @property {string} addFilter
 * @property {string} clearFilters
 * @property {string} columns
 * @property {string} refresh
 * @property {string} retry
 * @property {string} loading
 * @property {string} errorLoading
 * @property {string} noData
 * @property {string} noFilteredData
 * @property {string} previousPage
 * @property {string} nextPage
 * @property {string} rowsPerPage
 * @property {string} selectPage
 * @property {string} clearSelection
 * @property {string} restoreDefaults
 * @property {string} exportLabel
 * @property {string} moveUp
 * @property {string} moveDown
 * @property {string} field
 * @property {string} operator
 * @property {string} value
 * @property {string} from
 * @property {string} to
 * @property {string} apply
 * @property {string} remove
 * @property {(count: number) => string} selectedRows
 * @property {(from: number, to: number, total: number) => string} range
 * @property {(field: string) => string} sortAscLabel
 * @property {(field: string) => string} sortDescLabel
 * @property {(op: FilterOperatorValue) => string} operatorLabel
 */

/**
 * @template TData
 * @typedef {Object} SmartGridProps
 * @property {string} gridKey
 * @property {string} endpoint
 * @property {number} [pageSize]
 * @property {number[]} [pageSizeOptions]
 * @property {boolean} [searchable]
 * @property {boolean} [filterable]
 * @property {boolean} [sortable]
 * @property {boolean} [multiSortable]
 * @property {boolean} [selectable]
 * @property {"single"|"multiple"} [selectionMode]
 * @property {boolean} [syncUrl]
 * @property {boolean} [persistPreferences]
 * @property {number} [debounceMs]
 * @property {(row: TData) => string} [getRowId]
 * @property {GridColumnOverrides<TData>} [columns]
 * @property {GridColumnMetadata[]} [metadata]
 * @property {GridBulkAction<TData>[]} [bulkActions]
 * @property {GridExportOptions} [export]
 * @property {(rows: TData[], ids: ReadonlySet<string>) => void} [onSelectionChange]
 * @property {GridQueryOptions} [queryOptions]
 * @property {Partial<GridLocalization>} [localization]
 */

export {};
