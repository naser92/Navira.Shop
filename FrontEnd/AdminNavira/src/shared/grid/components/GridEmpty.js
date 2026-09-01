import { RiInboxLine } from "react-icons/ri";
import { Button } from "reactstrap";

export function GridEmpty({ hasSearchOrFilters, onClearFilters, localization }) {
  return (
    <div className="navira-grid-empty">
      <RiInboxLine size={28} />
      <p>
        {hasSearchOrFilters
          ? localization.noFilteredData
          : localization.noData}
      </p>
      {hasSearchOrFilters && (
        <Button color="secondary" size="sm" onClick={onClearFilters}>
          {localization.clearFilters}
        </Button>
      )}
    </div>
  );
}

GridEmpty.displayName = "GridEmpty";
