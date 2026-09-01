import { NextResponse } from "next/server";
import { callBackendWithAuth } from "@/lib/api/serverAuth";

export async function GET() {
  try {
    // This endpoint should be replaced with the actual backend endpoint
    const backendResponse = await callBackendWithAuth("/api/auth/UserAccessInfo", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!backendResponse.ok) {
      return NextResponse.json(
        {
          success: false,
          message: "Unauthorized",
        },
        { status: backendResponse.status }
      );
    }

    const result = await backendResponse.json();

    return NextResponse.json({
      success: true,
      data: result?.data || result,
    });
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: error.message || "Failed to fetch user access info",
      },
      { status: 500 }
    );
  }
}

// // Temporary mock implementation for development purposes
// export async function GET() {
//   // Simulate the expected API response structure
//   return NextResponse.json({
//     success: true,
//     data: {
//       // menus: [
//       //   {
//       //     permissionId: 1,
//       //     title: "مدیریت دسترسی",
//       //     route: "#",
//       //     sortOrder: 0,
//       //     childs: [
//       //       {
//       //         permissionId: 2,
//       //         title: "دسترسی ها",
//       //         route: "/access",
//       //         sortOrder: 0,
//       //         id: 2
//       //       }
//       //     ],
//       //     id: 1
//       //   },
//       //   {
//       //     permissionId: 3,
//       //     title: "داشبورد",
//       //     route: "/dashboard",
//       //     sortOrder: 1,
//       //     id: 3
//       //   },
//       //   {
//       //     permissionId: 4,
//       //     title: "کاربران",
//       //     route: "#",
//       //     sortOrder: 2,
//       //     childs: [
//       //       {
//       //         permissionId: 5,
//       //         title: "مدیریت کاربران",
//       //         route: "/users",
//       //         sortOrder: 0,
//       //         id: 5
//       //       },
//       //       {
//       //         permissionId: 6,
//       //         title: "گروه کاربری",
//       //         route: "/user-groups",
//       //         sortOrder: 1,
//       //         id: 6
//       //       }
//       //     ],
//       //     id: 4
//       //   }
//       // ]
//     }
//   });
// }
