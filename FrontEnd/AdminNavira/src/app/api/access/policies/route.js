import { NextResponse } from "next/server";
import { callBackendWithAuth } from "@/lib/api/serverAuth";

/**
 * Policy DTO matching backend PolicyDto (GET /api/Policy).
 * @typedef {Object} Policy
 * @property {number} id
 * @property {string} name
 * @property {string} title
 * @property {string} description
 * @property {boolean} isSystem
 * @property {boolean} isActive
 */

/**
 * Create policy request matching backend PolicyRegisterCommand (POST /api/Policy).
 * Backend accepts Content-Type: application/json-patch+json.
 * @typedef {Object} CreatePolicyRequest
 * @property {string|null} name
 * @property {string|null} title
 * @property {string|null} description
 * @property {boolean} isSystem
 * @property {boolean} isActive
 */

const normalizePolicies = (result) => {
  const payload = result?.data ?? result;
  if (Array.isArray(payload)) return payload;
  if (Array.isArray(payload?.items)) return payload.items;
  if (Array.isArray(payload?.data)) return payload.data;
  return [];
};

export async function GET(request) {
  try {
    const { search } = new URL(request.url);
    const backendResponse = await callBackendWithAuth(`/api/Policy${search}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });

    const result = await backendResponse.json().catch(() => null);

    // The backend wraps every response in { error, message, data } and returns
    // HTTP 200 even for business failures, so `error: true` must be treated as a failure.
    if (!backendResponse.ok || result?.error === true) {
      return NextResponse.json(
        { success: false, message: result?.message || "Failed to fetch policies" },
        { status: backendResponse.ok ? 400 : backendResponse.status }
      );
    }

    return NextResponse.json({ success: true, data: normalizePolicies(result) });
  } catch (error) {
    return NextResponse.json(
      { success: false, message: error.message || "Failed to fetch policies" },
      { status: 500 }
    );
  }
}

// Demo data kept for reference while the backend endpoint is being finalized:
// const DEMO_POLICIES = [
//   {
//     id: 1,
//     name: "دسترسی کامل",
//     description: "دسترسی نامحدود به تمام بخش‌های پنل مدیریت",
//     permissions: ["*.*.*"],
//   },
//   {
//     id: 2,
//     name: "دسترسی عملیاتی",
//     description: "مدیریت محصولات و سفارش‌ها بدون دسترسی به تنظیمات",
//     permissions: ["Get.Dashboard.Admin", "Get.Product.Admin", "Get.Order.Admin"],
//   },
//   {
//     id: 3,
//     name: "دسترسی مشاهده‌ای",
//     description: "فقط مشاهده گزارش‌ها و داشبورد",
//     permissions: ["Get.Dashboard.Admin"],
//   },
// ];

export async function POST(request) {
  try {
    const body = await request.json();
    const backendResponse = await callBackendWithAuth("/api/Policy", {
      method: "POST",
      headers: { "Content-Type": "application/json-patch+json" },
      body: JSON.stringify(body),
    });

    const result = await backendResponse.json().catch(() => null);

    // The backend wraps every response in { error, message, data } and returns
    // HTTP 200 even for business failures, so `error: true` must be treated as a failure.
    if (!backendResponse.ok || result?.error === true) {
      return NextResponse.json(
        { success: false, message: result?.message || "Failed to create policy" },
        { status: backendResponse.ok ? 400 : backendResponse.status }
      );
    }

    return NextResponse.json({ success: true, data: result?.data ?? result }, { status: 201 });
  } catch (error) {
    return NextResponse.json(
      { success: false, message: error.message || "Failed to create policy" },
      { status: 500 }
    );
  }
}
