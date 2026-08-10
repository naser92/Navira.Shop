# API Contract - NaviraShop

## 1. Overview
این سند، پروتکل ارتباطی بین کلاینت (Next.js) و سرویس بک‌اند (ASP.NET 9) را تعریف می‌کند. رعایت دقیق این قرارداد برای حفظ یکپارچگی داده‌ها، امنیت و هندلینگ صحیح خطاها الزامی است.

## 2. Base Configuration
تمام درخواست‌ها باید از طریق یک `Axios Instance` مرکزی که در `core/api/http-client.ts` تعریف شده، ارسال شوند.

- **Base URL:** محیط‌های مختلف (Development/Production) باید از طریق متغیرهای محیطی (`.env.local`) تنظیم شوند.
- **Headers:** تمام درخواست‌ها باید به صورت پیش‌فرض شامل هدرهای زیر باشند:
    - `Content-Type: application/json`
    - `Accept: application/json`
    - `Authorization: Bearer {token}` (تزریق خودکار توسط Interceptor)

## 3. Standard API Response
تمامی پاسخ‌های API (موفق یا ناموفق) باید از ساختار استاندارد زیر پیروی کنند. هوش مصنوعی باید همیشه این Interface را برای پاسخ‌های API در نظر بگیرد:
```typescript
export interface ApiResponse<T = any> {
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

## 4. Authentication & Security
- **Auth Provider:** استفاده از `Keycloak` جهت مدیریت نشست‌ها (Session).
- **JWT Handling:** توکن‌ها باید در یک فضای امن (HttpOnly Cookie یا Memory State) نگهداری شوند. 
- **Interceptor:** در `http-client.ts` یک Interceptor تعریف شده که:
- توکن را از Store/Context برداشته و در هدر Authorization قرار می‌دهد.
- در صورت دریافت خطای `401 Unauthorized`، کاربر را به مسیر `/login` هدایت می‌کند.
- در صورت دریافت خطای `403 Forbidden`، پیام دسترسی غیرمجاز نمایش می‌دهد.

## 5. Folder Structure for API
هر فیچر در پروژه باید APIهای اختصاصی خود را در مسیر `features/{feature-name}/api/` داشته باشد.

مثال برای سرویس محصول:
text
features/product/
├── api/
│   ├── product.service.ts    # توابع فراخوانی API
│   └── product.endpoints.ts  # آدرس‌های API (ثابت‌ها)
├── types/
│   └── product.dto.ts        # اینترفیس‌های داده‌ای

## 6. Implementation Rules for AI
هوش مصنوعی موظف است در هنگام تولید کدهای API، قواعد زیر را رعایت کند:

1. **Service vs Component:** هیچ API Call مستقیمی نباید در کامپوننت (Component) نوشته شود. همواره از متدهای تعریف شده در فایل‌های `service` استفاده شود.
2. **TanStack Query:** تمام درخواست‌های Fetching داده (GET) باید با `useQuery` یا `useInfiniteQuery` در لایه کامپوننت فراخوانی شوند. درخواست‌های تغییر داده (POST, PUT, DELETE) باید با `useMutation` مدیریت شوند.
3. **Type Safety:** هر endpoint باید دارای `ReturnType` (از جنس `ApiResponse<T>`) باشد. هرگز از `any` استفاده نکنید.
4. **Error Handling:** نباید `try/catch`های تکراری در کامپوننت نوشته شود. خطاها باید در `Axios Interceptor` یا `Mutation onError` مدیریت شوند.
5. **Path Consistency:** از نوشتن مستقیم URL در سرویس‌ها پرهیز کنید. URLها باید در فایل `endpoints.ts` هر فیچر ذخیره شوند.

## 7. Example Service (Template)
هوش مصنوعی باید از الگوی زیر برای تولید کد API پیروی کند:

typescript
// features/product/api/product.service.ts
import { apiClient } from "@/core/api/http-client";
import { ENDPOINTS } from "./product.endpoints";
import { Product } from "../types/product.dto";
import { ApiResponse } from "@/core/types/api-response";

export const getProductById = async (id: string): Promise<Product> => {
  const { data } = await apiClient.get<ApiResponse<Product>>(
ENDPOINTS.GET_BY_ID(id)
  );
  return data.data;
};

## 8. Error Codes & Handling
- **400 Bad Request:** نمایش پیام `errors` (در صورت وجود) در یک `Toast`.
- **401/403:** هدایت به صفحه لاگین یا نمایش پیام دسترسی.
- **500 Internal Server Error:** نمایش پیام عمومی "خطای سرور، لطفاً بعداً تلاش کنید".

## 9. Refit Integration (Optional)
در صورتی که پروژه از Refit استفاده می‌کند، تمام Interfaceهای API باید در `features/*/api/` تعریف شده و توسط Factory مرکزی ساخته شوند. (در حال حاضر اولویت با Axios است).

---


### چند نکته برای کارایی بیشتر:

1.  **یکپارچگی با Backend:** این قرارداد را به تیم بک‌اند بدهید تا مطمئن شوید آن‌ها هم دقیقا همین ساختار `ApiResponse<T>` را برمی‌گردانند (این یک استاندارد رایج در ASP.NET است).
2.  **استفاده از هوش مصنوعی:** اکنون شما سه داکیومنت اصلی (`PRD.md`, `ARCHITECTURE.md`, `DESIGN_SYSTEM.md`) و یک داکیومنت ارتباطی (`API_CONTRACT.md`) دارید.
3.  **شروع پیاده‌سازی:** اکنون می‌توانید به هوش مصنوعی بگویید:
> "بر اساس فایل‌های `docs/` که داریم، ساختار پوشه‌بندی و فایل `core/api/http-client.ts` را ایجاد کن."

آیا می‌خواهید برویم سراغ ایجاد اولین فایل پروژه (مثلاً `core/api/http-client.ts` و تنظیمات اولیه)؟