import { Input } from "reactstrap";

export function StringFilter({ value, onChange, disabled }) {
  return (
    <Input
      type="text"
      value={value ?? ""}
      onChange={(e) => onChange(e.target.value || undefined)}
      disabled={disabled}
      aria-label="filter text"
    />
  );
}
