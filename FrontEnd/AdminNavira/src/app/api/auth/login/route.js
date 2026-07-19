import { NextResponse } from "next/server";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL || "http://localhost:5979";

export async function POST(request) {
  try {
    const body = await request.json();

    const backendResponse = await fetch(`${API_BASE_URL}/api/auth/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json-patch+json",
      },
      body: JSON.stringify({
        username: body.username,
        password: body.password,
      }),
      cache: "no-store",
    });

    const result = await backendResponse.json();

    if (!backendResponse.ok || result?.error || !result?.data?.accessToken) {
      return NextResponse.json(
        {
          success: false,
          message: result?.message || "Login failed",
        },
        { status: backendResponse.status || 400 }
      );
    }

    const { accessToken, refreshToken, expiresIn, refreshExpiresIn } = result.data;

    const response = NextResponse.json({
      success: true,
      message: result?.message || "Login successful",
    });

    response.cookies.set("access_token", accessToken, {
      httpOnly: true,
      secure: process.env.NODE_ENV === "production",
      sameSite: "lax",
      path: "/",
      maxAge: Number(expiresIn || 3600),
    });

    response.cookies.set("refresh_token", refreshToken, {
      httpOnly: true,
      secure: process.env.NODE_ENV === "production",
      sameSite: "lax",
      path: "/",
      maxAge: Number(refreshExpiresIn || 604800),
    });

    return response;
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: error.message || "Unexpected error during login",
      },
      { status: 500 }
    );
  }
}
