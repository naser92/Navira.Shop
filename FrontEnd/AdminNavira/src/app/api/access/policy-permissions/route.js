import { NextResponse } from "next/server";
import { callBackendWithAuth } from "@/lib/api/serverAuth";

/**
 * Policy DTO (from GET /api/Policy).
 * @typedef {Object} Policy
 * @property {number} id
 * @property {string} name
 * @property {string} title
 * @property {string} description
 * @property {boolean} isSystem
 * @property {boolean} isActive
 */

/**
 * Permission DTO matching backend response (GET /api/Permission).
 * @typedef {Object} Permission
 * @property {number} id
 * @property {number} baseSubSystemId
 * @property {string} controllerName
 * @property {string} scope
 * @property {string} code
 * @property {string} title
 * @property {boolean} isActive
 */

/**
 * Policy-permission assignment request (POST /api/PlicyPermission).
 * Property names follow the backend contract exactly (including its spelling).
 * @typedef {Object} PolicyPermissionAssignmentRequest
 * @property {string|number} policyId
 * @property {Array<string|number>} permissionAsinge
 * @property {Array<string|number>} permissionUnAsinge
 */

const extractAssignedIds = (result) => {
  const payload = result?.data ?? result;
  const list = Array.isArray(payload)
    ? payload
    : Array.isArray(payload?.items)
      ? payload.items
      : Array.isArray(payload?.permissions)
        ? payload.permissions
        : [];
  return list
    .map((item) => (typeof item === "object" && item !== null ? item.id : item))
    .filter((value) => value !== null && value !== undefined);
};

const handleBackendResponse = async (backendResponse, fallbackMessage) => {
  const result = await backendResponse.json().catch(() => null);

  // The backend wraps every response in { error, message, data } and returns
  // HTTP 200 even for business failures, so `error: true` must be treated as a failure.
  if (!backendResponse.ok || result?.error === true) {
    return NextResponse.json(
      { success: false, message: result?.message || fallbackMessage },
      { status: backendResponse.ok ? 400 : backendResponse.status }
    );
  }

  return result;
};

export async function GET(request) {
  try {
    const { searchParams } = new URL(request.url);
    const policyId = searchParams.get("policyId");

    if (!policyId) {
      return NextResponse.json(
        { success: false, message: "policyId is required" },
        { status: 400 }
      );
    }

    const backendResponse = await callBackendWithAuth(`/api/PlicyPermission/${encodeURIComponent(policyId)}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });

    const result = await handleBackendResponse(backendResponse, "Failed to fetch policy permissions");
    if (result instanceof NextResponse) return result;

    return NextResponse.json({ success: true, data: extractAssignedIds(result) });
  } catch (error) {
    return NextResponse.json(
      { success: false, message: error.message || "Failed to fetch policy permissions" },
      { status: 500 }
    );
  }
}

export async function POST(request) {
  try {
    const body = await request.json().catch(() => null);

    if (!body || body.policyId === undefined || body.policyId === null) {
      return NextResponse.json(
        { success: false, message: "policyId is required" },
        { status: 400 }
      );
    }

    /** @type {PolicyPermissionAssignmentRequest} */
    const payload = {
      policyId: body.policyId,
      permissionAsinge: Array.isArray(body.permissionAsinge) ? body.permissionAsinge : [],
      permissionUnAsinge: Array.isArray(body.permissionUnAsinge) ? body.permissionUnAsinge : [],
    };

    const backendResponse = await callBackendWithAuth("/api/PlicyPermission", {
      method: "POST",
      headers: { "Content-Type": "application/json-patch+json" },
      body: JSON.stringify(payload),
    });

    const result = await handleBackendResponse(backendResponse, "Failed to save policy permissions");
    if (result instanceof NextResponse) return result;

    return NextResponse.json({ success: true, data: result?.data ?? null });
  } catch (error) {
    return NextResponse.json(
      { success: false, message: error.message || "Failed to save policy permissions" },
      { status: 500 }
    );
  }
}
