import type { ApiResponse } from "@/core/types/api-response";

// Normalized error shape surfaced to mutation/query `onError` handlers.
// Components must not build their own ad-hoc error shape (docs/API_CONTRACT.md section 6).
export class ApiError extends Error {
  public readonly statusCode: number;
  public readonly errors: string[];

  constructor(statusCode: number, message: string, errors: string[] = []) {
    super(message);
    this.name = "ApiError";
    this.statusCode = statusCode;
    this.errors = errors;
  }

  static fromApiResponse(statusCode: number, response?: Partial<ApiResponse<unknown>>): ApiError {
    const message = response?.message ?? "خطای سرور، لطفاً بعداً تلاش کنید";
    return new ApiError(statusCode, message, response?.errors ?? []);
  }
}
