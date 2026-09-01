import { RiSearchLine } from "react-icons/ri";
import { Input } from "reactstrap";

export function GridSearch({ value, onChange, localization }) {
  return (
    <div className="navira-grid-search">
      <RiSearchLine size={17} aria-hidden="true" />
      <Input
        type="text"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder={localization.searchPlaceholder}
        aria-label={localization.searchLabel}
      />
    </div>
  );
}

GridSearch.displayName = "GridSearch";
