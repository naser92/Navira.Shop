import { NextResponse } from "next/server";
// import { callBackendWithAuth } from "@/lib/api/serverAuth";

// TODO: replace demo data with the real backend call once the endpoint is ready.
// export async function GET(request) {
//   const { search } = new URL(request.url);
//   const backendResponse = await callBackendWithAuth(`/api/access/policies${search}`, {
//     method: "GET",
//     headers: { "Content-Type": "application/json" },
//   });
//   const result = await backendResponse.json();
//   return NextResponse.json({ success: true, data: result?.data ?? result });
// }

const DEMO_POLICIES = [
  {
    id: 1,
    name: "دسترسی کامل",
    description: "دسترسی نامحدود به تمام بخش‌های پنل مدیریت",
    permissions: ["*.*.*"],
  },
  {
    id: 2,
    name: "دسترسی عملیاتی",
    description: "مدیریت محصولات و سفارش‌ها بدون دسترسی به تنظیمات",
    permissions: ["Get.Dashboard.Admin", "Get.Product.Admin", "Get.Order.Admin"],
  },
  {
    id: 3,
    name: "دسترسی مشاهده‌ای",
    description: "فقط مشاهده گزارش‌ها و داشبورد",
    permissions: ["Get.Dashboard.Admin"],
  },
];

export async function GET() {
  return NextResponse.json({ success: true, data: DEMO_POLICIES });
}

// TODO: replace demo handler with the real backend call once the endpoint is ready.
// export async function POST(request) {
//   const body = await request.json();
//   const backendResponse = await callBackendWithAuth("/api/access/policies", {
//     method: "POST",
//     headers: { "Content-Type": "application/json" },
//     body: JSON.stringify(body),
//   });
//   const result = await backendResponse.json();
//   return NextResponse.json({ success: true, data: result?.data ?? result });
// }

export async function POST(request) {
  const body = await request.json().catch(() => ({}));

  if (!body?.name) {
    return NextResponse.json(
      { success: false, message: "نام سیاست الزامی است" },
      { status: 400 }
    );
  }

  return NextResponse.json(
    {
      success: true,
      data: {
        id: Date.now(),
        name: body.name,
        description: body.description || "",
        permissions: Array.isArray(body.permissions) ? body.permissions : [],
      },
    },
    { status: 201 }
  );
}
