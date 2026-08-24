"use client";

import { Badge } from "reactstrap";
import { RiErrorWarningLine } from "react-icons/ri";
import NaviraDataTable from "@/components/common/NaviraDataTable";
import { useAccessList } from "@/utils/hooks/access/useAccessCrud";

const permissionTexts = {
  Id: "شناسه",
  BaseSubSystemId: "شناسه زیرسیستم پایه",
  Controller: "کنترلر",
  Scope: "محدوده دسترسی",
  Code: "کد دسترسی",
  Title: "عنوان",
  Status: "وضعیت",
  Active: "فعال",
  Inactive: "غیرفعال",
  Empty: "مجوزی یافت نشد",
  Error: "خطا در دریافت مجوزها",
};

const PermissionList = () => {
  const { items: permissions, isLoading, error } = useAccessList("permissions");

  const columns = [
    { title: permissionTexts.Id, apiKey: "id", render: (row) => row.id },
    {
      title: permissionTexts.BaseSubSystemId,
      apiKey: "baseSubSystemId",
      render: (row) => row.baseSubSystemId,
    },
    {
      title: permissionTexts.Controller,
      apiKey: "controllerName",
      render: (row) => <span>{row.controllerName}</span>,
    },
    {
      title: permissionTexts.Scope,
      apiKey: "scope",
      render: (row) => <Badge color="primary" pill>{row.scope}</Badge>,
    },
    {
      title: permissionTexts.Code,
      apiKey: "code",
      render: (row) => <code className="navira-scope">{row.code}</code>,
    },
    {
      title: permissionTexts.Title,
      apiKey: "title",
      render: (row) => <strong>{row.title}</strong>,
    },
    {
      title: permissionTexts.Status,
      apiKey: "isActive",
      render: (row) => (
        <Badge color={row.isActive ? "success" : "danger"} pill>
          {row.isActive ? permissionTexts.Active : permissionTexts.Inactive}
        </Badge>
      ),
    },
  ];

  if (error) {
    return (
      <div className="navira-table" dir="rtl">
        <div className="navira-table-state text-danger">
          <RiErrorWarningLine size={22} />
          <span>{error.message || permissionTexts.Error}</span>
        </div>
      </div>
    );
  }

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
