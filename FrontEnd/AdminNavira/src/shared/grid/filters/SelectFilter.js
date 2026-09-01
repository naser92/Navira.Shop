import { Input } from "reactstrap";

/**
 * Select/enum filter. Options preserve their primitive type so the
 * serialized value matches the server contract exactly.
 */
export function SelectFilter({ value, onChange, disabled, options = [] }) {
  return (
    <Input
      type="select"
      value={value === undefined || value === null ? "" : JSON.stringify(value)}
      onChange={(e) => {
        const raw = e.target.value;
        if (raw === "") onChange(undefined);
        else {
          try {
            onChange(JSON.parse(raw));
          } catch {
            onChange(undefined);
          }
        }
      }}
      disabled={disabled}
      aria-label="filter select"
    >
      <option value="">—</option>
      {options.map((opt) => (
        <option key={JSON.stringify(opt.value)} value={JSON.stringify(opt.value)}>
          {opt.label}
        </option>
      ))}
    </Input>
  );
}
