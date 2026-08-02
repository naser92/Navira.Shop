"use client";

import { useState } from "react";
import { Badge } from "reactstrap";
import { RiLink, RiShieldUserLine } from "react-icons/ri";
import NaviraDataTable from "@/components/common/NaviraDataTable";
import { useAccessAssign, useAccessList } from "@/utils/hooks/access/useAccessCrud";
import ConnectionsModal from "./ConnectionsModal";
import { getItemId, getItemName, getItemPermissions, getPolicyIdsOfItem } from "./accessUtils";

const roleTexts = {
  Name: "نام نقش",
  Policies: "سیاست‌های متصل",
  Permissions: "مجوزها (از طریق سیاست‌ها)",
  Actions: "عملیات",
  ConnectPolicies: "اتصال سیاست‌ها",
  ConnectTitle: "اتصال سیاست‌ها به نقش",
  Empty: "نقشی یافت نشد",
  NoPolicy: "بدون سیاست",
};

const RoleList = () => {
  const { items: roles, isLoading } = useAccessList("roles");
  const { items: policies } = useAccessList("policies");
  const assign = useAccessAssign("roles");
  const [editing, setEditing] = useState(null);
  const policyIdsOf = (role) => getPolicyIdsOfItem(role);

  const policiesOf = (role) =>
    policies.filter((policy) =>
      policyIdsOf(role).map(String).includes(String(getItemId(policy)))
    );

  const permissionNamesOf = (role) =>
    [...new Set(policiesOf(role).flatMap((policy) => getItemPermissions(policy)))];

  const handleSave = (policyIds) => {
    assign.mutate(
      { id: getItemId(editing), body: { policies: policyIds } },
      { onSuccess: () => setEditing(null) }
    );
  };

  const columns = [
    { title: roleTexts.Name, apiKey: "name", render: (row) => <strong>{getItemName(row)}</strong> },
    {
      title: roleTexts.Policies,
      apiKey: "policies",
      render: (row) => {
        const linked = policiesOf(row);
        if (!linked.length) return <span>{roleTexts.NoPolicy}</span>;
        return (
          <div className="navira-chip-list">
            {linked.map((policy) => (
              <span key={getItemId(policy)} className="navira-chip">
                <RiShieldUserLine size={13} />
                {getItemName(policy)}
              </span>
            ))}
          </div>
        );
      },
    },
    {
      title: roleTexts.Permissions,
      apiKey: "permissions",
      render: (row) => <Badge color="info" pill>{permissionNamesOf(row).length}</Badge>,
    },
    {
      title: roleTexts.Actions,
      apiKey: "actions",
      render: (row) => (
        <button
          type="button"
          className="navira-action-btn"
          onClick={() => setEditing(row)}
          title={roleTexts.ConnectPolicies}
        >
          <RiLink size={16} />
          <span>{roleTexts.ConnectPolicies}</span>
        </button>
      ),
    },
  ];

  return (
    <>
      <NaviraDataTable
        columns={columns}
        data={roles}
        isLoading={isLoading}
        emptyMessage={roleTexts.Empty}
        searchPlaceholder="جستجوی نقش..."
      />
      <ConnectionsModal
        isOpen={Boolean(editing)}
        toggle={() => setEditing(null)}
        title={`${roleTexts.ConnectTitle} «${editing ? getItemName(editing) : ""}»`}
        options={policies}
        selectedIds={editing ? policyIdsOf(editing) : []}
        isSaving={assign.isPending}
        onSave={handleSave}
      />
    </>
  );
};

export default RoleList;
