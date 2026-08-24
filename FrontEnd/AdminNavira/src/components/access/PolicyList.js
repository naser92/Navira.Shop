"use client";

import { useState } from "react";
import { Badge, Button } from "reactstrap";
import { RiErrorWarningLine, RiKey2Line, RiLink, RiSettings3Line } from "react-icons/ri";
import NaviraDataTable from "@/components/common/NaviraDataTable";
import { useAccessAssign, useAccessList, usePolicyPermissions, useSavePolicyPermissions } from "@/utils/hooks/access/useAccessCrud";
import ConnectionsModal from "./ConnectionsModal";
import PolicyFormModal from "./PolicyFormModal";
import { getItemId, getItemName, getItemPermissions, getPolicyIdsOfItem } from "./accessUtils";
import Btn from "@/elements/buttons/Btn";

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
  ManagePermissions: "مدیریت مجوزها",
  ManagePermissionsTitle: "مدیریت مجوزهای سیاست",
};

const PolicyList = () => {
  const { items: policies, isLoading, error } = useAccessList("policies");
  const { items: roles } = useAccessList("roles");
  const { items: permissions } = useAccessList("permissions");
  const assign = useAccessAssign("policies");
  const [editing, setEditing] = useState(null);
  const [editingPolicy, setEditingPolicy] = useState(null);
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

  const policyPermissions = usePolicyPermissions(editingPolicy ? getItemId(editingPolicy) : null);
  const savePolicyPermissions = useSavePolicyPermissions();

  const handlePolicyPermissionsSave = (permissionIds) => {
    const policyId = getItemId(editingPolicy);
    const baseline = new Set(policyPermissions.items.map(String));
    const current = new Set(permissionIds.map(String));

    const permissionAsinge = [...current].filter((id) => !baseline.has(id));
    const permissionUnAsinge = [...baseline].filter((id) => !current.has(id));

    if (permissionAsinge.length === 0 && permissionUnAsinge.length === 0) {
      setEditingPolicy(null);
      return;
    }

    savePolicyPermissions.mutate(
      { policyId, permissionAsinge, permissionUnAsinge },
      { onSuccess: () => setEditingPolicy(null) }
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
        <div className="d-flex gap-2">
          <button
            type="button"
            className="navira-action-btn"
            onClick={() => setEditingPolicy(row)}
            title={policyTexts.ConnectPermissions}
          >
            <RiLink size={16} />
            <span>{policyTexts.ConnectPermissions}</span>
          </button>
      
        </div>
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
      <ConnectionsModal
        isOpen={Boolean(editingPolicy)}
        toggle={() => setEditingPolicy(null)}
        title={`${policyTexts.ManagePermissionsTitle} «${editingPolicy ? getItemName(editingPolicy) : ""}»`}
        description="مجوزهای مرتبط را انتخاب کنید"
        options={permissions}
        selectedIds={editingPolicy ? policyPermissions.items : []}
        isSaving={savePolicyPermissions.isPending || policyPermissions.isLoading}
        onSave={handlePolicyPermissionsSave}
      />
    </>
  );
};

export default PolicyList;
