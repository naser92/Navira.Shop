import { NextResponse } from "next/server";
// import { callBackendWithAuth } from "@/lib/api/serverAuth";

// TODO: replace demo data with the real backend call once the endpoint is ready.
// export async function GET(request) {
//   const { search } = new URL(request.url);
//   const backendResponse = await callBackendWithAuth(`/api/access/permissions${search}`, {
//     method: "GET",
//     headers: { "Content-Type": "application/json" },
//   });
//   const result = await backendResponse.json();
//   return NextResponse.json({ success: true, data: result?.data ?? result });
// }

const DEMO_PERMISSIONS = [
  { id: 1, name: "Get.Dashboard.Admin" },
  { id: 2, name: "Get.Product.Admin" },
  { id: 3, name: "Create.Product.Admin" },
  { id: 4, name: "Update.Product.Admin" },
  { id: 5, name: "Delete.Product.Admin" },
  { id: 6, name: "Get.User.Admin" },
  { id: 7, name: "Create.User.Admin" },
  { id: 8, name: "Update.User.Admin" },
  { id: 9, name: "Delete.User.Admin" },
  { id: 10, name: "Get.Order.Admin" },
  { id: 11, name: "Update.Order.Admin" },
  { id: 12, name: "Get.Role.Admin" },
  { id: 13, name: "Update.Role.Admin" },
];

export async function GET() {
  return NextResponse.json({ success: true, data: DEMO_PERMISSIONS });
}
