"use client";

import { useMemo, useState } from "react";
import { Input, Spinner, Table } from "reactstrap";
import { RiInboxLine, RiSearchLine } from "react-icons/ri";

const NaviraDataTable = ({
  columns,
  data = [],
  isLoading = false,
  searchable = true,
  searchPlaceholder = "جستجو...",
  emptyMessage = "موردی یافت نشد",
  pageSize = 10,
  toolbarActions = null,
}) => {
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const filtered = useMemo(() => {
    if (!search.trim()) return data;
    const term = search.trim().toLowerCase();
    return data.filter((row) =>
      columns.some((col) => String(row[col.apiKey] ?? "").toLowerCase().includes(term))
    );
  }, [data, search, columns]);

  const totalPages = Math.max(1, Math.ceil(filtered.length / pageSize));
  const safePage = Math.min(page, totalPages);
  const rows = filtered.slice((safePage - 1) * pageSize, safePage * pageSize);

  const handleSearch = (event) => {
    setSearch(event.target.value);
    setPage(1);
  };

  return (
    <div className="navira-table" dir="rtl">
      {(searchable || toolbarActions) && (
        <div className="navira-table-toolbar">
          {searchable && (
            <div className="navira-table-search">
              <RiSearchLine size={17} />
              <Input
                type="text"
                value={search}
                onChange={handleSearch}
                placeholder={searchPlaceholder}
                aria-label={searchPlaceholder}
              />
            </div>
          )}
          <div className="navira-table-toolbar-end">
            <span className="navira-table-count">{filtered.length} مورد</span>
            {toolbarActions}
          </div>
        </div>
      )}

      <div className="table-responsive">
        <Table hover className="navira-table-grid mb-0">
          <thead>
            <tr>
              <th className="navira-table-serial">#</th>
              {columns.map((col) => (
                <th key={col.apiKey}>{col.title}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {isLoading ? (
              <tr>
                <td colSpan={columns.length + 1} className="navira-table-state">
                  <Spinner size="sm" color="primary" />
                  <span>در حال بارگذاری...</span>
                </td>
              </tr>
            ) : rows.length === 0 ? (
              <tr>
                <td colSpan={columns.length + 1} className="navira-table-state">
                  <RiInboxLine size={22} />
                  <span>{emptyMessage}</span>
                </td>
              </tr>
            ) : (
              rows.map((row, index) => (
                <tr key={row.id ?? index}>
                  <td className="navira-table-serial">{(safePage - 1) * pageSize + index + 1}</td>
                  {columns.map((col) => (
                    <td key={col.apiKey} data-label={col.title}>
                      {col.render ? col.render(row) : String(row[col.apiKey] ?? "—")}
                    </td>
                  ))}
                </tr>
              ))
            )}
          </tbody>
        </Table>
      </div>

      {totalPages > 1 && (
        <div className="navira-table-pagination">
          <button
            type="button"
            className="navira-page-btn"
            disabled={safePage <= 1}
            onClick={() => setPage(safePage - 1)}
          >
            قبلی
          </button>
          {Array.from({ length: totalPages }, (_, i) => i + 1).map((pageNumber) => (
            <button
              key={pageNumber}
              type="button"
              className={`navira-page-btn ${pageNumber === safePage ? "active" : ""}`}
              onClick={() => setPage(pageNumber)}
            >
              {pageNumber}
            </button>
          ))}
          <button
            type="button"
            className="navira-page-btn"
            disabled={safePage >= totalPages}
            onClick={() => setPage(safePage + 1)}
          >
            بعدی
          </button>
        </div>
      )}
    </div>
  );
};

export default NaviraDataTable;
