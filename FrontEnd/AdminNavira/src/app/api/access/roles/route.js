import { NextResponse } from "next/server";
import { callBackendWithAuth } from "@/lib/api/serverAuth";

/**
 * Role DTO matching backend KeycloakRoleDto (GET /api/Roles).
 * @typedef {Object} Role
 * @property {string} id
 * @property {string} name
 * @property {string} [description]
 * @property {boolean} [composite]
 * @property {boolean} [clientRole]
 * @property {string} [containerId]
 */

const normalizeRoles = (result) => {
  const payload = result?.data ?? result;
  if (Array.isArray(payload)) return payload;
  if (Array.isArray(payload?.items)) return payload.items;
  if (Array.isArray(payload?.data)) return payload.data;
  return [];
};

export async function GET(request) {
  try {
    const { search } = new URL(request.url);
    const backendResponse = await callBackendWithAuth(`/api/Roles${search}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });

    const result = await backendResponse.json().catch(() => null);

    // The backend wraps every response in { error, message, data } and returns
    // HTTP 200 even for business failures, so `error: true` must be treated as a failure.
    if (!backendResponse.ok || result?.error === true) {
      return NextResponse.json(
        { success: false, message: result?.message || "Failed to fetch roles" },
        { status: backendResponse.ok ? 400 : backendResponse.status }
      );
    }

    return NextResponse.json({ success: true, data: normalizeRoles(result) });
  } catch (error) {
    return NextResponse.json(
      { success: false, message: error.message || "Failed to fetch roles" },
      { status: 500 }
    );
  }
}

// Demo data kept for reference while the backend endpoint is being finalized:
// const DEMO_ROLES = [
//   { id: 1, name: "مدیر ارشد", policyIds: [1, 2] },
//   { id: 2, name: "مدیر محصولات", policyIds: [2] },
//   { id: 3, name: "پشتیبان فروش", policyIds: [2, 3] },
//   { id: 4, name: "بازدیدکننده", policyIds: [3] },
// ];
