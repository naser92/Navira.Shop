import { Button, Input } from "reactstrap";
import { paginationRange } from "../core/utils.js";

export function GridPagination({
  currentPage,
  totalPages,
  totalCount,
  pageSize,
  onPageChange,
  onPageSizeChange,
  pageSizeOptions,
  localization,
}) {
  const { from, to } = paginationRange(currentPage, pageSize, totalCount);

  const pageOptions = pageSizeOptions || [10, 20, 50, 100];

  const pageNumbers = [];
  const maxVisible = 5;
  const halfVisible = Math.floor(maxVisible / 2);
  
  let startPage = Math.max(1, currentPage - halfVisible);
  let endPage = Math.min(totalPages, currentPage + halfVisible);
  
  if (endPage - startPage + 1 < maxVisible) {
    if (startPage === 1) {
      endPage = Math.min(totalPages, startPage + maxVisible - 1);
    } else {
      startPage = Math.max(1, endPage - maxVisible + 1);
    }
  }

  for (let i = startPage; i <= endPage; i++) {
    pageNumbers.push(i);
  }

  return (
    <div className="navira-grid-pagination">
      <div className="navira-grid-pagination-info">
        {localization.range(from, to, totalCount)}
      </div>

      <div className="navira-grid-pagination-controls">
        <Button
          size="sm"
          disabled={currentPage <= 1}
          onClick={() => onPageChange(currentPage - 1)}
        >
          {localization.previousPage}
        </Button>

        {startPage > 1 && (
          <>
            <Button size="sm" onClick={() => onPageChange(1)}>1</Button>
            {startPage > 2 && <span>...</span>}
          </>
        )}

        {pageNumbers.map(page => (
          <Button
            key={page}
            size="sm"
            color={page === currentPage ? "primary" : "outline-primary"}
            onClick={() => onPageChange(page)}
          >
            {page}
          </Button>
        ))}

        {endPage < totalPages && (
          <>
            {endPage < totalPages - 1 && <span>...</span>}
            <Button size="sm" onClick={() => onPageChange(totalPages)}>
              {totalPages}
            </Button>
          </>
        )}

        <Button
          size="sm"
          disabled={currentPage >= totalPages}
          onClick={() => onPageChange(currentPage + 1)}
        >
          {localization.nextPage}
        </Button>
      </div>

      <div className="navira-grid-pagination-size">
        <label>{localization.rowsPerPage}:</label>
        <Input
          type="select"
          value={pageSize}
          onChange={(e) => onPageSizeChange(Number(e.target.value))}
        >
          {pageOptions.map(size => (
            <option key={size} value={size}>{size}</option>
          ))}
        </Input>
      </div>
    </div>
  );
}
