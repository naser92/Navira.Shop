import { NextResponse } from "next/server";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL || "http://localhost:5979";

export async function POST(request) {
  try {
    const body = await request.json();

    const backendResponse = await fetch(`${API_BASE_URL}/api/auth/forgot-password`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json-patch+json",
      },
      body: JSON.stringify({
        email: body.email,
      }),
      cache: "no-store",
    });

    const result = await backendResponse.json();

    if (!backendResponse.ok || result?.error) {
      return NextResponse.json(
        {
          success: false,
          message: result?.message || "Forgot password request failed",
        },
        { status: backendResponse.status || 400 }
      );
    }

    return NextResponse.json({
      success: true,
      message: result?.message || "Reset link sent successfully",
    });
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: error.message || "Unexpected error during forgot password",
      },
      { status: 500 }
    );
  }
}
