"use client";

import { useState } from "react";
import { Badge } from "reactstrap";
import { RiErrorWarningLine, RiKey2Line, RiLink } from "react-icons/ri";
import NaviraDataTable from "@/components/common/NaviraDataTable";
import { useAccessAssign, useAccessList } from "@/utils/hooks/access/useAccessCrud";
import ConnectionsModal from "./ConnectionsModal";
import PolicyFormModal from "./PolicyFormModal";
import { getItemId, getItemName, getItemPermissions, getPolicyIdsOfItem } from "./accessUtils";

const policyTexts = {
  Name: "نام سیاست",
  Description: "توضیحات",
  Permissions: "مجوزها",
  Roles: "نقش‌های متصل",
  Actions: "عملیات",
  ConnectPermissions: "اتصال مجوزها",
  ConnectTitle: "اتصال مجوزها به سیاست",
  Empty: "سیاستی یافت نشد",
  Error: "خطا در دریافت سیاست‌ها",
  NoDescription: "—",
  NoPermission: "—",
};

const PolicyList = () => {
  const { items: policies, isLoading, error } = useAccessList("policies");
  const { items: roles } = useAccessList("roles");
  const { items: permissions } = useAccessList("permissions");
  const assign = useAccessAssign("policies");
  const [editing, setEditing] = useState(null);
  const permissionNamesOf = (policy) => getItemPermissions(policy);

  const roleCountOf = (policy) =>
    roles.filter((role) =>
      getPolicyIdsOfItem(role).map(String).includes(String(getItemId(policy)))
    ).length;

  const handleSave = (permissionNames) => {
    assign.mutate(
      { id: getItemId(editing), body: { permissions: permissionNames } },
      { onSuccess: () => setEditing(null) }
    );
  };

  const columns = [
    { title: policyTexts.Name, apiKey: "name", render: (row) => <strong>{getItemName(row)}</strong> },
    {
      title: policyTexts.Description,
      apiKey: "description",
      render: (row) => row.description || policyTexts.NoDescription,
    },
    {
      title: policyTexts.Permissions,
      apiKey: "permissions",
      render: (row) => {
        const names = permissionNamesOf(row);
        if (!names.length) return <span>{policyTexts.NoPermission}</span>;
        return (
          <div className="navira-chip-list">
            {names.slice(0, 3).map((name) => (
              <span key={name} className="navira-chip">
                <RiKey2Line size={13} />
                {name}
              </span>
            ))}
            {names.length > 3 && <Badge color="info" pill>+{names.length - 3}</Badge>}
          </div>
        );
      },
    },
    {
      title: policyTexts.Roles,
      apiKey: "roles",
      render: (row) => <Badge color="primary" pill>{roleCountOf(row)}</Badge>,
    },
    {
      title: policyTexts.Actions,
      apiKey: "actions",
      render: (row) => (
        <button
          type="button"
          className="navira-action-btn"
          onClick={() => setEditing(row)}
          title={policyTexts.ConnectPermissions}
        >
          <RiLink size={16} />
          <span>{policyTexts.ConnectPermissions}</span>
        </button>
      ),
    },
  ];

  if (error) {
    return (
      <div className="navira-table" dir="rtl">
        <div className="navira-table-state text-danger">
          <RiErrorWarningLine size={22} />
          <span>{error.message || policyTexts.Error}</span>
        </div>
      </div>
    );
  }

  return (
    <>
      <NaviraDataTable
        columns={columns}
        data={policies}
        isLoading={isLoading}
        emptyMessage={policyTexts.Empty}
        searchPlaceholder="جستجوی سیاست..."
        toolbarActions={<PolicyFormModal />}
      />
      <ConnectionsModal
        isOpen={Boolean(editing)}
        toggle={() => setEditing(null)}
        title={`${policyTexts.ConnectTitle} «${editing ? getItemName(editing) : ""}»`}
        options={permissions.map((permission) => ({ id: getItemName(permission), name: getItemName(permission) }))}
        selectedIds={editing ? permissionNamesOf(editing) : []}
        isSaving={assign.isPending}
        onSave={handleSave}
      />
    </>
  );
};

export default PolicyList;
