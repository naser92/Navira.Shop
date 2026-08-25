# AI Frontend Project Context

## 1. Project Summary
- **Name**: `navira-admin` (پنل مدیریت فروشگاه نویرا)
- **Type**: Admin Panel / Dashboard
- **Role**: Frontend مدیریت فروشگاه اینترنتی Navira (کاربران، محصولات، سفارش‌ها، فروشگاه‌ها و ...). بر پایه قالب آماده Fastkart Admin (Pixelstrap) خریداری‌شده و در حال بازنویسی مطابق بیزینس Navira. قالب اصلی در `bot/template/` نگه داشته شده و فقط به‌عنوان مرجع بصری/الگویی استفاده می‌شود — منطق آن کپی نمی‌شود.
- **Backend**: REST API جداگانه (پیش‌فرض `http://localhost:5979`) که از طریق الگوی BFF با Route Handlerهای `src/app/api/auth/*` به آن متصل می‌شود.
- **Auth**: Custom JWT (accessToken + refreshToken) ذخیره‌شده در **httpOnly cookie** توسط سرور Next. بدون Keycloak/NextAuth.

## 2. Technology Stack

| Area | Technology | Notes |
|---|---|---|
| Framework | Next.js 16.2.10 | App Router، Turbopack |
| Language | JavaScript (JSX) | بدون TypeScript؛ path alias `@/* -> ./src/*` در `jsconfig.json` |
| React | 19.2.4 | |
| Package manager | npm | `package-lock.json` موجود |
| UI library | Reactstrap 9 + Bootstrap 5.3 | آیکون: react-icons/remixicon + react-feather |
| Styling | SCSS (sass 1.77) | ورودی: `public/assets/scss/app.scss` → `style.scss`؛ overrideهای پروژه در `layout/_navira-layout.scss` |
| State management | React Context + useReducer | `src/helper/accountContext`, `src/helper/settingContext` |
| Data fetching | fetch (BFF) + TanStack Query v5 | `src/lib/api/clientApi.js` برای auth؛ wrapper قالب با axios در `src/utils/axiosUtils/` (legacy) |
| Forms | Formik 2.4 | wrapperها: `src/components/reactstrapFormik/` |
| Validation | Yup 0.32 | اسکیماها: `src/utils/validation/ValidationSchemas.js` |
| Auth | Custom JWT + httpOnly cookies | BFF routes در `src/app/api/auth/` |
| i18n | i18next + react-i18next 15 | موجود ولی تا حد زیادی بلااستفاده؛ متن‌ها فارسی hardcode |
| Notifications | react-toastify 11 | wrapper فارسی RTL: `src/lib/toast/` |
| Lint | ESLint 10 + eslint-config-next 16 | بدون فایل config جدا (Next built-in) |
| Formatter | Prettier | Not found |
| Testing | — | Not found |
| Deployment | Vercel-ready | `vercel.json` (CORS headers برای `/api/*`) |

## 3. Routing Architecture
- **App Router** (`src/app/`) با route groups: `(authLayout)` و `(mainLayout)`.
- Root layout: `src/app/layout.js` (client؛ فراهم‌کننده QueryClient, SettingProvider, AccountProvider, ToastProvider؛ `<html dir="rtl" lang="fa">`).
- `not-found.js` در root app: Not found (حذف شده/وجود ندارد).
- `src/proxy.js`: proxy/middleware خالی با matcher روی `/` — عملاً غیرفعال (بدهی فنی).

| Route | Path/File | Type | Protected | Notes |
|---|---|---|---|---|
| `/` | `src/app/(mainLayout)/page.js` + redirect در `next.config.js` | static | بله | redirect به `/dashboard` |
| `/dashboard` | `src/app/(mainLayout)/dashboard/page.js` | static | بله | صفحه خوش‌آمد فارسی |
| `/profile` | `src/app/(mainLayout)/profile/page.js` | static | بله | placeholder پروفایل |
| `/access` | `src/app/(mainLayout)/access/page.js` | static | بله | مدیریت دسترسی‌ها: تب‌های سیاست/نقش/مجوز |
| `/page` | `src/app/(mainLayout)/page/page.js` | static | بله | placeholder تستی |
| `/auth/login` | `src/app/(authLayout)/auth/login/page.js` | static | خیر | فرم ورود Formik |
| `/auth/forgot-password` | `src/app/(authLayout)/auth/forgot-password/page.js` | static | خیر | |
| `/api/auth/*` | `src/app/api/auth/{login,logout,refresh,me,forgot-password}/route.js` | dynamic (BFF) | — | پراکسی به Backend |
| `/api/access/*` | `src/app/api/access/{policies,roles,permissions,role-policies,policy-permissions}/route.js` | dynamic (BFF) | — | **فعلاً داده دمو برمی‌گرداند؛** نسخه واقعی (callBackendWithAuth) در کامنت |

- **Protection**: گارد client-side در `src/app/(mainLayout)/layout.js` — کامپوننت `ProtectedContent` که با `AccountContext.isAuthenticated/isLoading` کار می‌کند و به `/auth/login` ریدایرکت می‌کند. Middleware سمت سرور وجود ندارد.

## 4. Folder Structure

| Path | Purpose | Key Files |
|---|---|---|
| `src/app/` | App Router: routeها، layouts، BFF API routes، i18n | `layout.js`, `(mainLayout)/layout.js` |
| `src/app/api/auth/` | BFF route handlerها برای احراز هویت | `login/route.js`, `refresh/route.js`, `me/route.js` |
| `src/components/` | کامپوننت‌های shared | `common/PermissionGuard.js`, `reactstrapFormik/` |
| `src/elements/` | UI primitiveهای قالب (Buttons, Inputs, ...) | `src/elements/` |
| `src/helper/` | Context providers و utilities | `accountContext`, `settingContext` |
| `src/utils/` | Helper functions و hooks | `src/utils/hooks/access/` |
| `src/lib/` | API clients و utilities | `src/lib/api/` |

## 5. Access Management Architecture

### 5.1. Data Flow
1. **Frontend → BFF**: Client components call `apiFetch` in `src/lib/api/clientApi.js` که به `/api/access/*` می‌رود.
2. **BFF → Backend**: Route Handlerها با `callBackendWithAuth` از `src/lib/api/serverAuth.js` به backend واقعی (`http://localhost:5979`) متصل می‌شوند.
3. **Authentication**: Bearer token از httpOnly cookie خوانده می‌شود.

### 5.2. BFF Routes
| Route | Backend Endpoint | Method | Purpose |
|---|---|---|---|
| `/api/access/roles` | `/api/Role` | GET | لیست نقش‌ها |
| `/api/access/roles/{id}` | `/api/Role/{id}` | PUT | به‌روزرسانی نقش |
| `/api/access/policies` | `/api/Policy` | GET | لیست سیاست‌ها |
| `/api/access/policies` | `/api/Policy` | POST | ایجاد سیاست |
| `/api/access/policies/{id}` | `/api/Policy/{id}` | PUT | به‌روزرسانی سیاست |
| `/api/access/permissions` | `/api/Permission` | GET | لیست مجوزها |
| `/api/access/role-policies` | `/api/RolePolicy` | GET/POST | اتصال نقش به سیاست |
| `/api/access/policy-permissions` | `/api/PlicyPermission` | GET/POST | اتصال سیاست به مجوز |

### 5.3. Component Structure
```
src/components/access/
├── PolicyList.js           # لیست سیاست‌ها + اتصال مجوز
├── RoleList.js             # لیست نقش‌ها + اتصال سیاست
├── PermissionList.js       # لیست مجوزها
├── ConnectionsModal.js     # مودال اتصال (Role→Policy, Policy→Permission)
├── PolicyFormModal.js      # فرم ایجاد/ویرایش سیاست
└── accessUtils.js          # helper functions
```

### 5.4. Hooks Architecture
```
src/utils/hooks/access/useAccessCrud.js
├── useAccessList(resource)         # لیست (roles, policies, permissions)
├── useAccessAssign(resource)       # PUT برای به‌روزرسانی
├── useAccessCreate(resource)       # POST برای ایجاد
├── useRolePolicies(roleId)         # GET /api/RolePolicy/{roleId}
├── useSaveRolePolicies()           # POST /api/RolePolicy
├── usePolicyPermissions(policyId)  # GET /api/PlicyPermission/{policyId}
└── useSavePolicyPermissions()      # POST /api/PlicyPermission
```

### 5.5. Assignment Pattern
- **Baseline**: لیست اولیه از سرور (immutable)
- **Current**: انتخاب‌های کاربر (editable)
- **Diff**: محاسبه با `Set` — `Asinge` = selected − baseline, `UnAsinge` = baseline − selected
- **No-op**: اگر تغییری نباشد، POST ارسال نمی‌شود

## 6. Role-Policy Assignment (2026-08-23)
- BFF route `src/app/api/access/role-policies/route.js`: GET proxies `GET /api/RolePolicy/{roleId}` (extracts assigned policy IDs from `{data}` envelope, supports ID or object items), POST proxies `POST /api/RolePolicy` with `Content-Type: application/json-patch+json` and exact backend payload `{ roleId, policyAsinge, policyUnAsinge }` (backend spelling preserved). Auth: Bearer from httpOnly cookie via `callBackendWithAuth`.
- Hooks in `src/utils/hooks/access/useAccessCrud.js`: `useRolePolicies(roleId)` (TanStack Query, cached per role, no duplicate fetches) and `useSaveRolePolicies()` (success toast + invalidates `["access","role-policies"]` and `["access","roles"]`).
- `RoleList`: opens ConnectionsModal per role; baseline = assigned IDs from `useRolePolicies`; on save computes Set difference (policyAsinge = selected − assigned, policyUnAsinge = assigned − selected); no-op when unchanged (closes without POST); failure keeps modal open via existing toast error handling.
- `ConnectionsModal`: added client-side search (name/title/code/description, case-insensitive, Persian-safe, selection preserved while filtering); save/cancel disabled while saving.

## 7. Policy-Permission Assignment (2026-08-23)
- BFF route `src/app/api/access/policy-permissions/route.js`: GET proxies `GET /api/PlicyPermission/{policyId}` (extracts assigned permission IDs from `{data}` envelope, supports ID or object items), POST proxies `POST /api/PlicyPermission` with `Content-Type: application/json-patch+json` and exact backend payload `{ policyId, permissionAsinge, permissionUnAsinge }` (backend spelling preserved). Auth: Bearer from httpOnly cookie via `callBackendWithAuth`.
- Hooks in `src/utils/hooks/access/useAccessCrud.js`: `usePolicyPermissions(policyId)` (TanStack Query, cached per policy, no duplicate fetches) and `useSavePolicyPermissions()` (success toast + invalidates `["access","policy-permissions"]` and `["access","policies"]`).
- `PolicyList`: opens ConnectionsModal per policy for permission management; baseline = assigned IDs from `usePolicyPermissions`; on save computes Set difference (permissionAsinge = selected − assigned, permissionUnAsinge = assigned − selected); no-op when unchanged (closes without POST); failure keeps modal open via existing toast error handling.
- `ConnectionsModal`: reused existing component with client-side search (name/title/code/description, case-insensitive, Persian-safe, selection preserved while filtering); save/cancel disabled while saving.

## 8. Error Handling
- **401**: `apiFetch` → refresh token → retry → logout on failure
- **403**: `apiFetch` → redirect to `/403`
- **Other**: Toast error با message backend

## 9. Current Limitations
- Backend endpoints ممکن است در حال توسعه باشند (demo data در BFF routes)
- TypeScript در حال حاضر استفاده نمی‌شود (JavaScript با JSDoc)
- تست واحد وجود ندارد

## 10. Development Notes
- **Backend URL**: `http://localhost:5979` (از `src/lib/api/serverAuth.js`)
- **Path alias**: `@/*` → `./src/*`
- **RTL**: همه کامپوننت‌ها `dir="rtl"` دارند
- **Persian**: تمام UI text به فارسی است

## Product Creation Page Implementation (2026-08-24)
- Route: `src/app/(mainLayout)/products/new/page.js` - 5-step wizard for creating new e-commerce products
- Steps: Basic Info → Attributes → Variants → Images → Inventory → Review
- Components: Created reusable components in `src/components/products/`:
  - ProductStepper: Horizontal stepper for navigation
  - ProductBasicInfoStep: Product details, descriptions, status
  - ProductAttributesStep: Attribute/value management with add/remove
  - ProductVariantsStep: Variant table with CRUD operations, modals
  - ProductImagesStep: Drag-and-drop image upload for product/variants
  - ProductInventoryStep: Warehouse inventory management with calculations
  - ProductReviewStep: Final product summary before submission
- Features: RTL Persian UI, form validation, state persistence between steps, image previews, 
  inventory calculations (Available = Initial - Reserved), cartesion product generation,
  variant combinations, and complete product object creation
- Validation: Required field checks, duplicate SKU prevention, price validation,
  required variant count, attribute combination uniqueness
- Dependencies: Used existing project stack (React, Reactstrap, ReactFeather, Next.js)
- State management: Local component state with React hooks
- Mock data: Pre-populated sample product (Stanley Mug) with variants
- Output: Complete product object logged to console on submission
- Design: Clean, minimal SaaS dashboard UI with Persian typography
- Navigation: Wizard with step validation, sticky footer, progress tracking
- Accessibility: Keyboard navigable, proper labeling, screen reader friendly
- Responsive: Desktop-first with mobile adaptations for modals and tables
