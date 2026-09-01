"use client";

/** @typedef {import("../core/types.js").SmartGridProps<unknown>} SmartGridProps */

import { useEffect, useMemo, useRef, useState } from "react";
import { Table, Input, Button, Dropdown, DropdownToggle, DropdownMenu, ButtonGroup } from "reactstrap";
import { FaSort, FaSortUp, FaSortDown, FaCaretUp, FaCaretDown } from "react-icons/fa";
import { useGrid } from "../hooks/useGrid.js";
import { resolveRowId } from "../core/utils.js";
import { resolveColumns } from "../core/column-utils.js";
import { fa as faLocales } from "../localization/index.js";
import { GridLoading } from "./GridLoading.js";
import { GridEmpty } from "./GridEmpty.js";
import { GridError } from "./GridError.js";
import { GridSearch } from "./GridSearch.js";
import { GridFilters } from "./GridFilters.js";
import { GridPagination } from "./GridPagination.js";
import { GridColumnManager } from "./GridColumnManager.js";
import { GridBulkActions } from "./GridBulkActions.js";
import { useGridQuery } from "../hooks/useGridQuery.js";
import { useGridSort } from "../hooks/useGridSort.js";

/**
 * Production-ready, reusable, generic Data Grid framework.
 * Server-side pagination, search, filtering, sorting with Persian RTL support.
 *
 * @template TData
 * @param {SmartGridProps<TData>} props
 */
export function SmartGrid(props) {
  const {
    gridKey,
    endpoint,
    pageSize: pageSizeProp,
    pageSizeOptions,
    searchable = true,
    filterable = true,
    sortable = true,
    multiSortable = false,
    selectable = false,
    selectionMode = "multiple",
    syncUrl = false,
    persistPreferences = true,
    debounceMs,
    getRowId,
    columns: columnOverrides,
    metadata: propMetadata,
    bulkActions,
    export: exportOpts,
    onSelectionChange,
    queryOptions,
    localization: propLocales,
  } = props;

  const localization = { ...faLocales, ...propLocales };

  // Central state
  const {
    page,
    pageSize,
    searchInput,
    search,
    filters,
    sorts,
    request,
    preferences,
    preferencesHydrated,
    setPage,
    setPageSize,
    setSearchInput,
    setFilters,
    setSorts,
    clearFiltersAndSearch,
    persistAll,
    resetPreferences,
  } = useGrid({
    gridKey,
    pageSize: pageSizeProp,
    syncUrl,
    persistPreferences,
    debounceMs,
  });

  // Server query
  const query = useGridQuery({
    gridKey,
    endpoint,
    request,
    queryOptions,
  });

  // Response metadata resolution
  const { metadata: responseMetadata } = query.data || {};
  const { metadata: resolvedMetadata } = useMemo(
    () => {
      // Backend metadata takes precedence, then prop metadata
      if (Array.isArray(responseMetadata) && responseMetadata.length > 0) {
        return { metadata: responseMetadata };
      }
      if (Array.isArray(propMetadata) && propMetadata.length > 0) {
        return { metadata: propMetadata };
      }
      return { metadata: [] };
    },
    [responseMetadata, propMetadata]
  );

  // Column resolution
  const { columns, order, visible, widths, overrides } = useMemo(
    () => resolveColumns(resolvedMetadata, preferences, columnOverrides),
    [resolvedMetadata, preferences, columnOverrides]
  );

  // Column state persistence
  const persistColumnState = useMemo(
    () => ({
      visibleColumns: Array.from(visible),
      columnOrder: order,
      columnWidths: widths,
    }),
    [visible, order, widths]
  );
  useEffect(() => { persistAll(persistColumnState); }, [persistAll, persistColumnState]);

  // Sorting interactions
  const { toggleSort, getSortState } = useGridSort({
    sorts,
    setSorts,
    multiSort: multiSortable,
  });

  // Selection
  const [selectedIds, setSelectedIds] = useState(new Set());
  const selectedIdsRef = useRef(selectedIds);

  useEffect(() => {
    selectedIdsRef.current = selectedIds;
  }, [selectedIds]);
  const [dropdownOpen, setDropdownOpen] = useState(false);

  const toggleDropdown = () => setDropdownOpen((prevState) => !prevState);

  const toggleRowSelection = (row, id) => {
    const newSelected = new Set(selectedIds);
    if (newSelected.has(id)) {
      newSelected.delete(id);
    } else if (selectionMode === "single") {
      newSelected.clear();
      newSelected.add(id);
    } else {
      newSelected.add(id);
    }
    setSelectedIds(newSelected);
    selectedIdsRef.current = newSelected;
    if (onSelectionChange) {
      const selectedRows = query.data?.items?.filter(item => {
        const itemId = resolveRowId(item, getRowId);
        return newSelected.has(itemId);
      }) || [];
      onSelectionChange(selectedRows, Object.freeze(newSelected));
    }
  };

  const togglePageSelection = () => {
    if (!query.data?.items) return;
    const pageIds = query.data.items.map(item => resolveRowId(item, getRowId)).filter(Boolean);
    if (pageIds.every(id => selectedIds.has(id))) {
      // Deselect all on page
      const newSelected = new Set(selectedIds);
      pageIds.forEach(id => newSelected.delete(id));
      setSelectedIds(newSelected);
      selectedIdsRef.current = newSelected;
    } else {
      // Select all on page
      const newSelected = new Set(selectedIds);
      pageIds.forEach(id => newSelected.add(id));
      setSelectedIds(newSelected);
      selectedIdsRef.current = newSelected;
    }
  };

  const clearSelection = () => {
    setSelectedIds(new Set());
    selectedIdsRef.current = new Set();
    if (onSelectionChange) {
      onSelectionChange([], Object.freeze(new Set()));
    }
  };

  // Derived state
  const hasSearchOrFilters = !!(search || filters.length > 0);
  const isLoading = query.isLoading;
  const isRefetching = query.isRefetching;
  const hasError = !!query.error;
  const hasData = query.data?.items && query.data.items.length > 0;

  // Render
  if (hasError) {
    return (
      <div className="navira-grid navira-grid-error-container">
        <GridError
          onRetry={() => query.refetch()}
          localization={localization}
        />
      </div>
    );
  }

  return (
    <div className="navira-grid">
      {/* Toolbar */}
      <div className="navira-grid-toolbar">
        {selectable && selectedIds.size > 0 && (
          <div className="navira-grid-selected-info">
            {localization.selectedRows(selectedIds.size)}
            <Button size="sm" color="link" onClick={clearSelection}>
              {localization.clearSelection}
            </Button>
          </div>
        )}
        
        <div className="navira-grid-toolbar-content">
          {searchable && (
            <GridSearch
              value={searchInput}
              onChange={setSearchInput}
              localization={localization}
            />
          )}

          <div className="navira-grid-toolbar-actions">
            {filterable && (
              <GridFilters
                columns={columns}
                filters={filters}
                setFilters={setFilters}
                localization={localization}
              />
            )}

            {selectable && bulkActions && bulkActions.length > 0 && (
              <GridBulkActions
                actions={bulkActions}
                selectedIds={selectedIds}
                localization={localization}
              />
            )}

            <GridColumnManager
              columns={columns}
              order={order}
              visible={visible}
              widths={widths}
              setOrder={(newOrder) => persistAll({ ...persistColumnState, columnOrder: newOrder })}
              setVisible={(newVisible) => persistAll({ ...persistColumnState, visibleColumns: Array.from(newVisible) })}
              setWidths={(newWidths) => persistAll({ ...persistColumnState, columnWidths: newWidths })}
              localization={localization}
            />

            <Button
              size="sm"
              color="outline-secondary"
              onClick={() => query.refetch()}
              title={localization.refresh}
            >
              ↻
            </Button>

            {exportOpts?.enabled && (
              <Dropdown isOpen={dropdownOpen} toggle={toggleDropdown}>
                <DropdownToggle color="outline-secondary" size="sm" caret>
                  {localization.exportLabel}
                </DropdownToggle>
                <DropdownMenu>
                  {(exportOpts.formats || ['csv', 'excel']).map(format => (
                    <Button
                      key={format}
                      size="sm"
                      color="link"
                      onClick={() => {
                        if (exportOpts.onExport) {
                          exportOpts.onExport({
                            request: { search, filters, sorts },
                            format,
                            gridKey,
                            endpoint,
                          });
                        }
                      }}
                    >
                      {format.toUpperCase()}
                    </Button>
                  ))}
                </DropdownMenu>
              </Dropdown>
            )}
          </div>
        </div>
      </div>

      {/* Loading overlay */}
      <GridLoading
        isLoading={isLoading}
        isRefetching={isRefetching}
        localization={localization}
      />

      {/* Main grid */}
      <div className="table-responsive navira-grid-table-container">
        <Table hover className="navira-grid-table mb-0">
          <thead>
            <tr>
              {selectable && (
                <th className="navira-grid-select-header">
                  <Input
                    type="checkbox"
                    checked={query.data?.items?.length > 0 && query.data.items.every(item => {
                      const id = resolveRowId(item, getRowId);
                      return id && selectedIds.has(id);
                    })}
                    onChange={togglePageSelection}
                    aria-label={localization.selectPage}
                  />
                </th>
              )}
              {order.map(field => {
                const column = columns.find(c => c.field === field);
                if (!column || !visible.has(field)) return null;

                const { direction, priority } = getSortState(field);
                const canSort = sortable && column.sortable !== false;

                return (
                  <th
                    key={field}
                    style={{ width: widths[field] }}
                    onClick={canSort ? () => toggleSort(field, true) : undefined}
                    className={canSort ? 'navira-grid-sortable-header' : ''}
                    aria-sort={direction === 'asc' ? 'ascending' : direction === 'desc' ? 'descending' : 'none'}
                  >
                    <div className="navira-grid-header-content">
                      <span>{column.title}</span>
                      {canSort && direction && (
                        <span className="navira-grid-sort-indicator">
                          {direction === 'asc' ? <FaSortUp /> : <FaSortDown />}
                          {priority && priority > 1 && (
                            <small className="navira-grid-sort-priority">{priority}</small>
                          )}
                        </span>
                      )}
                    </div>
                  </th>
                );
              })}
            </tr>
          </thead>
          <tbody>
            {!hasData && !isLoading && (
              <tr>
                <td colSpan={order.length + (selectable ? 1 : 0)} className="navira-grid-empty-cell">
                  <GridEmpty
                    hasSearchOrFilters={hasSearchOrFilters}
                    onClearFilters={clearFiltersAndSearch}
                    localization={localization}
                  />
                </td>
              </tr>
            )}
            {hasData && query.data.items.map((row, index) => {
              const id = resolveRowId(row, getRowId);
              const isSelected = id ? selectedIds.has(id) : false;

              return (
                <tr key={id || index} className={isSelected ? 'navira-grid-row-selected' : ''}>
                  {selectable && (
                    <td className="navira-grid-select-cell">
                      <Input
                        type="checkbox"
                        checked={isSelected}
                        onChange={() => id && toggleRowSelection(row, id)}
                        aria-label={`Select row ${index + 1}`}
                      />
                    </td>
                  )}
                  {order.map(field => {
                    const column = columns.find(c => c.field === field);
                    if (!column || !visible.has(field)) return null;

                    const value = row[field];
                    const override = overrides[field];
                    
                    return (
                      <td key={field} className="navira-grid-data-cell">
                        {override?.render
                          ? override.render({ value, row, rowIndex: index, column })
                          : String(value ?? '—')}
                      </td>
                    );
                  })}
                </tr>
              );
            })}
          </tbody>
        </Table>
      </div>

      {/* Pagination */}
      {query.data && (
        <GridPagination
          currentPage={query.data.page}
          totalPages={query.data.totalPages}
          totalCount={query.data.totalCount}
          pageSize={query.data.pageSize}
          onPageChange={setPage}
          onPageSizeChange={setPageSize}
          pageSizeOptions={pageSizeOptions}
          localization={localization}
        />
      )}
    </div>
  );
}

SmartGrid.displayName = "SmartGrid";
