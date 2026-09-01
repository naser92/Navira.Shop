import { Input } from "reactstrap";

export function BooleanFilter({ value, onChange, disabled }) {
  return (
    <Input
      type="select"
      value={value === undefined || value === null ? "" : String(value)}
      onChange={(e) => {
        const v = e.target.value;
        if (v === "") onChange(undefined);
        else onChange(v === "true");
      }}
      disabled={disabled}
      aria-label="filter boolean"
    >
      <option value="">—</option>
      <option value="true">فعال</option>
      <option value="false">غیرفعال</option>
    </Input>
  );
}
