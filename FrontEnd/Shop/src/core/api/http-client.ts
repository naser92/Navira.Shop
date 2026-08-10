import axios, { type AxiosError, type InternalAxiosRequestConfig } from "axios";

import { env } from "@/core/config/env";
import { getAccessToken, notifyUnauthorized } from "@/core/auth/token-store";
import { ApiError } from "@/core/api/api-error";
import type { ApiResponse } from "@/core/types/api-response";

// Central Axios instance. docs/ARCHITECTURE.md and docs/API_CONTRACT.md
// require every API call in the app to go through this client. Creating a
// separate `axios.create` instance inside a feature is not allowed.
export const apiClient = axios.create({
  baseURL: env.apiBaseUrl,
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json",
  },
});

// Request interceptor: injects the Keycloak access token, when present.
apiClient.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  const token = getAccessToken();

  if (token) {
    config.headers.set("Authorization", `Bearer ${token}`);
  }

  return config;
});

// Response interceptor: unwraps standard errors and centralizes 401/403 handling
// so components/services never need repeated try/catch blocks
// (docs/API_CONTRACT.md sections 4 and 8).
apiClient.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ApiResponse<unknown>>) => {
    const statusCode = error.response?.status ?? 0;
    const apiResponse = error.response?.data;

    if (statusCode === 401) {
      notifyUnauthorized();
    }

    return Promise.reject(ApiError.fromApiResponse(statusCode, apiResponse));
  },
);
