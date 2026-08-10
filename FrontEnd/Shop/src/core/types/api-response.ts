// Standard API response contract shared by every ASP.NET endpoint.
// See docs/API_CONTRACT.md section 3. Do not redefine this shape per feature.

export interface ApiResponse<T> {
  data: T;
  succeeded: boolean;
  message?: string;
  errors?: string[];
  statusCode: number;
}

export interface PagedResponse<T> extends ApiResponse<T[]> {
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
