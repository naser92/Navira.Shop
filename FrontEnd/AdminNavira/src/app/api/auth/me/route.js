import { NextResponse } from "next/server";
import { callBackendWithAuth } from "@/lib/api/serverAuth";

// export async function GET() {
//   try {
//     // این مسیر را با endpoint واقعی خودت تنظیم کن
//     const backendResponse = await callBackendWithAuth("/api/auth/me", {
//       method: "GET",
//       headers: {
//         "Content-Type": "application/json",
//       },
//     });

//     if (!backendResponse.ok) {
//       return NextResponse.json(
//         {
//           success: false,
//           message: "Unauthorized",
//         },
//         { status: backendResponse.status }
//       );
//     }

//     const result = await backendResponse.json();

//     return NextResponse.json({
//       success: true,
//       data: result?.data || result,
//     });
//   } catch (error) {
//     return NextResponse.json(
//       {
//         success: false,
//         message: error.message || "Failed to fetch current user",
//       },
//       { status: 500 }
//     );
//   }
// }


export async function GET() {
  return NextResponse.json({
    success: true,
    data: {
      userInfo: {
        id: 1,
        userName: "demo",
        fullName: "Demo User",
      },
      userAccess: ["Get.Dashboard.Admin"],
    },
  });
}