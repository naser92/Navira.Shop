import { useState } from "react";
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from "reactstrap";
import Toast from "@/lib/toast/index.js";

export function GridBulkActions({ actions, selectedIds, localization }) {
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const [pendingAction, setPendingAction] = useState(null);

  const toggle = () => setDropdownOpen(!dropdownOpen);

  const executeAction = async (action) => {
    if (pendingAction) return;
    setPendingAction(action.id);
    try {
      const result = await action.action({
        selectedRows: [],
        selectedIds: Object.freeze(selectedIds),
      });
      if (result?.selection === "clear") {
        // Parent owns clearing.
      }
      Toast.success(`عملیات ${action.label} با موفقیت انجام شد`);
    } catch (error) {
      Toast.error(error.message || `خطا در انجام عملیات ${action.label}`);
    } finally {
      setPendingAction(null);
      setDropdownOpen(false);
    }
  };

  if (selectedIds.size === 0) return null;

  return (
    <Dropdown isOpen={dropdownOpen} toggle={toggle}>
      <DropdownToggle color="outline-primary" size="sm" caret>
        {localization.selectedRows(selectedIds.size)}
      </DropdownToggle>
      <DropdownMenu>
        {actions.map((action) => (
          <DropdownItem
            key={action.id}
            onClick={() => {
              if (
                action.confirm &&
                !window.confirm(
                  typeof action.confirm === "string"
                    ? action.confirm
                    : `آیا از انجام عملیات "${action.label}" اطمینان دارید؟`
                )
              ) {
                return;
              }
              executeAction(action);
            }}
            disabled={action.disabled || pendingAction === action.id}
            className={action.destructive ? "text-danger" : ""}
          >
            {action.label}
          </DropdownItem>
        ))}
      </DropdownMenu>
    </Dropdown>
  );
}
