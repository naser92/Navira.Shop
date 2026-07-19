export function hasPermission(userAccess = [], permission) {
  if (!permission) return true;
  if (!Array.isArray(userAccess)) return false;

  return userAccess.includes(permission);
}

export function hasAnyPermission(userAccess = [], permissions = []) {
  if (!permissions?.length) return true;
  return permissions.some((item) => hasPermission(userAccess, item));
}

export function hasAllPermissions(userAccess = [], permissions = []) {
  if (!permissions?.length) return true;
  return permissions.every((item) => hasPermission(userAccess, item));
}
