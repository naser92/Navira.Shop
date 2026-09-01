import { Input } from "reactstrap";

export function DateFilter({ value, onChange, disabled }) {
  return (
    <Input
      type="date"
      value={value ?? ""}
      onChange={(e) => onChange(e.target.value || undefined)}
      disabled={disabled}
      aria-label="filter date"
    />
  );
}
