import { NextResponse } from "next/server";
import { callBackendWithAuth } from "@/lib/api/serverAuth";

export async function GET(request) {
  try {
    const incomingAuthHeader = request?.headers?.get?.("authorization");

    const headers = {
      "Content-Type": "application/json",
    };

    if (incomingAuthHeader) {
      headers.Authorization = incomingAuthHeader;
    }

    const backendResponse = await callBackendWithAuth("/api/auth/UserAccessInfo", {
      method: "GET",
      headers,
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
