"use client";

import { useContext } from "react";
import AccountContext from "@/helper/accountContext/accountContext";
import { hasPermission, hasAnyPermission, hasAllPermissions } from "@/lib/auth/permissions";

export default function PermissionGuard({
  permission,
  anyOf,
  allOf,
  fallback = null,
  children,
}) {
  const account = useContext(AccountContext);
  const userAccess = account?.userAccess || [];

  let allowed = true;

  if (permission) {
    allowed = hasPermission(userAccess, permission);
  }

  if (allowed && anyOf?.length) {
    allowed = hasAnyPermission(userAccess, anyOf);
  }

  if (allowed && allOf?.length) {
    allowed = hasAllPermissions(userAccess, allOf);
  }

  if (!allowed) return fallback;

  return children;
}
