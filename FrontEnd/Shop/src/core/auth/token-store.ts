// In-memory access token store.
//
// docs/ARCHITECTURE.md and docs/API_CONTRACT.md require the JWT to be kept in
// a secure location (HttpOnly cookie or memory state), never in localStorage.
// This module exposes a minimal, framework-agnostic accessor so that the
// http-client interceptor can read the current token without depending on a
// specific auth feature implementation. The `auth` feature (Keycloak
// integration) is responsible for calling `setAccessToken` / `clearAccessToken`.

let accessToken: string | null = null;

export function getAccessToken(): string | null {
  return accessToken;
}

export function setAccessToken(token: string | null): void {
  accessToken = token;
}

export function clearAccessToken(): void {
  accessToken = null;
}

export type UnauthorizedHandler = () => void;

let unauthorizedHandler: UnauthorizedHandler | null = null;

export function setUnauthorizedHandler(handler: UnauthorizedHandler | null): void {
  unauthorizedHandler = handler;
}

export function notifyUnauthorized(): void {
  unauthorizedHandler?.();
}
