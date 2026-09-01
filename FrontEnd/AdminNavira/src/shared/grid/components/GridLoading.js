import { Spinner } from "reactstrap";

/**
 * Loading states:
 * - Initial: spinner + full skeleton (table dimensions preserved)
 * - Background refetch: subtle indicator, rows stay visible
 */
export function GridLoading({ isLoading, isRefetching, localization }) {
  if (!isLoading && !isRefetching) return null;
  return (
    <div
      className={`navira-grid-loading ${
        isLoading ? "navira-grid-initial-loading" : "navira-grid-refetching"
      }`}
      aria-live="polite"
      aria-busy="true"
    >
      {isLoading && (
        <div className="navira-grid-initial-overlay">
          <Spinner size="sm" color="primary" />
          <span>{localization.loading}</span>
        </div>
      )}
      {isRefetching && !isLoading && (
        <div className="navira-grid-refetch-indicator" />
      )}
    </div>
  );
}

GridLoading.displayName = "GridLoading";
