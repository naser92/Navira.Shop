import { NextResponse } from "next/server";
import { callBackendWithAuth } from "@/lib/api/serverAuth";

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

const normalizePermissions = (result) => {
  const payload = result?.data ?? result;
  if (Array.isArray(payload)) return payload;
  if (Array.isArray(payload?.items)) return payload.items;
  if (Array.isArray(payload?.data)) return payload.data;
  return [];
};

export async function GET(request) {
  try {
    const { search } = new URL(request.url);
    const backendResponse = await callBackendWithAuth(`/api/Permission${search}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });

    const result = await backendResponse.json().catch(() => null);

    // The backend wraps every response in { error, message, data } and returns
    // HTTP 200 even for business failures, so `error: true` must be treated as a failure.
    if (!backendResponse.ok || result?.error === true) {
      return NextResponse.json(
        { success: false, message: result?.message || "Failed to fetch permissions" },
        { status: backendResponse.ok ? 400 : backendResponse.status }
      );
    }

    return NextResponse.json({ success: true, data: normalizePermissions(result) });
  } catch (error) {
    return NextResponse.json(
      { success: false, message: error.message || "Failed to fetch permissions" },
      { status: 500 }
    );
  }
}
