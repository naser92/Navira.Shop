"use client";

import { Badge } from "reactstrap";
import { RiApps2Line, RiFunctionLine, RiShieldCheckLine } from "react-icons/ri";
import NaviraDataTable from "@/components/common/NaviraDataTable";
import { useAccessList } from "@/utils/hooks/access/useAccessCrud";
import {
  getItemId,
  getItemName,
  getItemPermissions,
  getPolicyIdsOfItem,
  matchesPermission,
  parseScope,
} from "./accessUtils";

const permissionTexts = {
  Name: "نام مجوز",
  Method: "متد",
  Controller: "کنترلر",
  App: "اپ",
  UsedBy: "متصل به",
  Empty: "مجوزی یافت نشد",
};

const METHOD_COLORS = {
  get: "success",
  post: "primary",
  put: "warning",
  patch: "warning",
  delete: "danger",
};

const PermissionList = () => {
  const { items: permissions, isLoading } = useAccessList("permissions");
  const { items: policies } = useAccessList("policies");
  const { items: roles } = useAccessList("roles");

  const usedBy = (permissionName) => {
    const holders = [];

    policies.forEach((policy) => {
      if (matchesPermission(permissionName, getItemPermissions(policy))) {
        holders.push(`سیاست: ${getItemName(policy)}`);
      }
    });

    const matchedPolicyIds = policies
      .filter((policy) => matchesPermission(permissionName, getItemPermissions(policy)))
      .map((policy) => String(getItemId(policy)));

    roles.forEach((role) => {
      const rolePolicyIds = getPolicyIdsOfItem(role).map(String);
      if (rolePolicyIds.some((id) => matchedPolicyIds.includes(id))) {
        holders.push(`نقش: ${getItemName(role)}`);
      }
    });

    return [...new Set(holders)];
  };

  const columns = [
    { title: permissionTexts.Name, apiKey: "name", render: (row) => <code className="navira-scope">{getItemName(row)}</code> },
    {
      title: permissionTexts.Method,
      apiKey: "method",
      render: (row) => {
        const { method } = parseScope(row.name);
        return <Badge color={METHOD_COLORS[method.toLowerCase()] || "secondary"} pill>{method || "—"}</Badge>;
      },
    },
    {
      title: permissionTexts.Controller,
      apiKey: "controller",
      render: (row) => (
        <span className="navira-scope-part">
          <RiFunctionLine size={14} />
          {parseScope(row.name).controller || "—"}
        </span>
      ),
    },
    {
      title: permissionTexts.App,
      apiKey: "app",
      render: (row) => (
        <span className="navira-scope-part">
          <RiApps2Line size={14} />
          {parseScope(row.name).app || "—"}
        </span>
      ),
    },
    {
      title: permissionTexts.UsedBy,
      apiKey: "usedBy",
      render: (row) => {
        const holders = usedBy(row.name);
        if (!holders.length) return <span>—</span>;
        return (
          <div className="navira-chip-list">
            {holders.slice(0, 2).map((label) => (
              <span key={label} className="navira-chip">
                <RiShieldCheckLine size={13} />
                {label}
              </span>
            ))}
            {holders.length > 2 && <Badge color="info" pill>+{holders.length - 2}</Badge>}
          </div>
        );
      },
    },
  ];

  return (
    <NaviraDataTable
      columns={columns}
      data={permissions}
      isLoading={isLoading}
      emptyMessage={permissionTexts.Empty}
      searchPlaceholder="جستجوی مجوز..."
    />
  );
};

export default PermissionList;
