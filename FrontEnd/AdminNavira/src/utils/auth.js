export function setAuthCookies(data) {
  document.cookie = `uat=${encodeURIComponent(data.accessToken)}; path=/; max-age=${data.expiresIn}; samesite=lax`;
  document.cookie = `urt=${encodeURIComponent(data.refreshToken)}; path=/; max-age=${data.refreshExpiresIn}; samesite=lax`;
  document.cookie = `utt=${encodeURIComponent(data.tokenType || "Bearer")}; path=/; max-age=${data.expiresIn}; samesite=lax`;
}

export function getCookie(name) {
  if (typeof document === "undefined") return null;
  const match = document.cookie.match(new RegExp("(^| )" + name + "=([^;]+)"));
  return match ? decodeURIComponent(match[2]) : null;
}

export function clearAuthCookies() {
  document.cookie = "uat=; path=/; max-age=0";
  document.cookie = "urt=; path=/; max-age=0";
  document.cookie = "utt=; path=/; max-age=0";
}
