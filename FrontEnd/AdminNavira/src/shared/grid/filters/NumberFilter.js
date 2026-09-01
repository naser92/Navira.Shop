import { Input } from "reactstrap";

export function NumberFilter({ value, onChange, disabled }) {
  return (
    <Input
      type="number"
      value={value ?? ""}
      onChange={(e) => {
        const raw = e.target.value;
        if (raw === "") onChange(undefined);
        else {
          const n = Number(raw);
          if (Number.isFinite(n)) onChange(n);
        }
      }}
      disabled={disabled}
      aria-label="filter number"
    />
  );
}
