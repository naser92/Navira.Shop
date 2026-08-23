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
 * Role-policy save request (POST /api/RolePolicy).
 * Property names follow the backend contract exactly (including its spelling).
 * @typedef {Object} RolePolicyAssignmentRequest
 * @property {string|number} roleId
 * @property {Array<string|number>} policyAsinge
 * @property {Array<string|number>} policyUnAsinge
 */

const extractAssignedIds = (result) => {
  const payload = result?.data ?? result;
  const list = Array.isArray(payload)
    ? payload
    : Array.isArray(payload?.items)
      ? payload.items
      : Array.isArray(payload?.policies)
        ? payload.policies
        : [];
  return list
    .map((item) => (typeof item === "object" && item !== null ? (item.id ?? item.policyId) : item))
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
    const roleId = searchParams.get("roleId");

    if (!roleId) {
      return NextResponse.json(
        { success: false, message: "roleId is required" },
        { status: 400 }
      );
    }

    const backendResponse = await callBackendWithAuth(`/api/RolePolicy/${encodeURIComponent(roleId)}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });

    const result = await handleBackendResponse(backendResponse, "Failed to fetch role policies");
    if (result instanceof NextResponse) return result;

    return NextResponse.json({ success: true, data: extractAssignedIds(result) });
  } catch (error) {
    return NextResponse.json(
      { success: false, message: error.message || "Failed to fetch role policies" },
      { status: 500 }
    );
  }
}

export async function POST(request) {
  try {
    const body = await request.json().catch(() => null);

    if (!body || body.roleId === undefined || body.roleId === null) {
      return NextResponse.json(
        { success: false, message: "roleId is required" },
        { status: 400 }
      );
    }

    /** @type {RolePolicyAssignmentRequest} */
    const payload = {
      roleId: body.roleId,
      policyAsinge: Array.isArray(body.policyAsinge) ? body.policyAsinge : [],
      policyUnAsinge: Array.isArray(body.policyUnAsinge) ? body.policyUnAsinge : [],
    };

    const backendResponse = await callBackendWithAuth("/api/RolePolicy", {
      method: "POST",
      headers: { "Content-Type": "application/json-patch+json" },
      body: JSON.stringify(payload),
    });

    const result = await handleBackendResponse(backendResponse, "Failed to save role policies");
    if (result instanceof NextResponse) return result;

    return NextResponse.json({ success: true, data: result?.data ?? null });
  } catch (error) {
    return NextResponse.json(
      { success: false, message: error.message || "Failed to save role policies" },
      { status: 500 }
    );
  }
}
