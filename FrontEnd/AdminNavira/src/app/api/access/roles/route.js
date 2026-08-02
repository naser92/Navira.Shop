import { NextResponse } from "next/server";
// import { callBackendWithAuth } from "@/lib/api/serverAuth";

// TODO: replace demo data with the real backend call once the endpoint is ready.
// export async function GET(request) {
//   const { search } = new URL(request.url);
//   const backendResponse = await callBackendWithAuth(`/api/access/roles${search}`, {
//     method: "GET",
//     headers: { "Content-Type": "application/json" },
//   });
//   const result = await backendResponse.json();
//   return NextResponse.json({ success: true, data: result?.data ?? result });
// }

const DEMO_ROLES = [
  { id: 1, name: "مدیر ارشد", policyIds: [1, 2] },
  { id: 2, name: "مدیر محصولات", policyIds: [2] },
  { id: 3, name: "پشتیبان فروش", policyIds: [2, 3] },
  { id: 4, name: "بازدیدکننده", policyIds: [3] },
];

export async function GET() {
  return NextResponse.json({ success: true, data: DEMO_ROLES });
}
