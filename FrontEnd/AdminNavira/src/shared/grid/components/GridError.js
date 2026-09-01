import { RiErrorWarningLine } from "react-icons/ri";
import { Button } from "reactstrap";

export function GridError({ onRetry, localization }) {
  return (
    <div className="navira-grid-error">
      <RiErrorWarningLine size={28} />
      <p>{localization.errorLoading}</p>
      <Button color="secondary" size="sm" onClick={onRetry}>
        {localization.retry}
      </Button>
    </div>
  );
}

GridError.displayName = "GridError";
