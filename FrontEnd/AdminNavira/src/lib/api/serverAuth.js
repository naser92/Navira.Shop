import { cookies } from "next/headers";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL || "http://localhost:5979";

export async function callBackend(url, options = {}) {
  const response = await fetch(`${API_BASE_URL}${url}`, {
    ...options,
    cache: "no-store",
  });

  return response;
}

export async function getAccessTokenFromCookies() {
  const cookieStore = await cookies();
  return cookieStore.get("access_token")?.value || null;
}

export async function getRefreshTokenFromCookies() {
  const cookieStore = await cookies();
  return cookieStore.get("refresh_token")?.value || null;
}

export async function callBackendWithAuth(url, options = {}) {
  const accessToken = await getAccessTokenFromCookies();

  const headers = new Headers(options.headers || {});
  if (accessToken) {
    headers.set("Authorization", `Bearer ${accessToken}`);
  }

  const response = await fetch(`${API_BASE_URL}${url}`, {
    ...options,
    headers,
    cache: "no-store",
  });

  return response;
}
