export const getItemId = (item) => item?.id ?? item?._id ?? item?.uuid ?? item?.name;

export const getItemName = (item) =>
  item?.name ?? item?.title ?? item?.fullName ?? item?.userName ?? String(getItemId(item) ?? "");

export const parseScope = (scope = "") => {
  const [method = "", controller = "", app = ""] = String(scope).split(".");
  return { method, controller, app };
};

export const getItemPermissions = (item) => {
  const raw = item?.permissions ?? item?.permission ?? item?.userAccess ?? item?.scopes ?? [];
  if (!Array.isArray(raw)) return [];
  return raw.map((perm) => (typeof perm === "string" ? perm : perm?.name)).filter(Boolean);
};

export const getPolicyIdsOfItem = (item) => {
  const raw = item?.policyIds ?? item?.policy_ids ?? item?.policies ?? item?.policyId ?? item?.policy_id ?? item?.policy ?? [];
  const list = Array.isArray(raw) ? raw : [raw];
  return list.map((policy) => (typeof policy === "object" && policy !== null ? getItemId(policy) : policy)).filter((v) => v != null);
};

export const getPolicyIdOfItem = (item) => getPolicyIdsOfItem(item)[0] ?? null;

export const getRoleIdsOfItem = (item) => {
  const roles = item?.roles ?? item?.role ?? [];
  const list = Array.isArray(roles) ? roles : [roles];
  return list.map((role) => (typeof role === "object" ? getItemId(role) : role)).filter((v) => v != null);
};

export const collectPermissionsFromRoles = (roleIds, roles) =>
  roles
    .filter((role) => roleIds.map(String).includes(String(getItemId(role))))
    .flatMap((role) => getItemPermissions(role));

export const matchesPermission = (permissionName, itemPermissions) =>
  itemPermissions.some(
    (perm) =>
      perm === permissionName ||
      perm === "*.*.*" ||
      (perm.endsWith(".*.*") && permissionName.startsWith(perm.split(".")[0] + "."))
  );
