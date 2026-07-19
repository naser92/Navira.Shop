import { NextResponse } from "next/server";
import { cookies } from "next/headers";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL || "http://localhost:5979";

export async function POST() {
  try {
    const cookieStore = await cookies();
    const refreshToken = cookieStore.get("refresh_token")?.value;

    if (!refreshToken) {
      return NextResponse.json(
        { success: false, message: "Refresh token not found" },
        { status: 401 }
      );
    }

    const backendResponse = await fetch(`${API_BASE_URL}/api/auth/refresh-token`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json-patch+json",
      },
      body: JSON.stringify({
        refreshToken,
      }),
      cache: "no-store",
    });

    const result = await backendResponse.json();

    if (!backendResponse.ok || result?.error || !result?.data?.accessToken) {
      return NextResponse.json(
        { success: false, message: result?.message || "Refresh failed" },
        { status: backendResponse.status || 401 }
      );
    }

    const { accessToken, refreshToken: newRefreshToken, expiresIn, refreshExpiresIn } = result.data;

    const response = NextResponse.json({
      success: true,
      message: "Token refreshed",
    });

    response.cookies.set("access_token", accessToken, {
      httpOnly: true,
      secure: process.env.NODE_ENV === "production",
      sameSite: "lax",
      path: "/",
      maxAge: Number(expiresIn || 3600),
    });

    response.cookies.set("refresh_token", newRefreshToken, {
      httpOnly: true,
      secure: process.env.NODE_ENV === "production",
      sameSite: "lax",
      path: "/",
      maxAge: Number(refreshExpiresIn || 604800),
    });

    return response;
  } catch (error) {
    return NextResponse.json(
      { success: false, message: error.message || "Refresh error" },
      { status: 500 }
    );
  }
}
