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
| `/api/access/*` | `src/app/api/access/{policies,roles,permissions}/route.js` | dynamic (BFF) | — | **فعلاً داده دمو برمی‌گرداند؛** نسخه واقعی (callBackendWithAuth) در کامنت |

- **Protection**: گارد client-side در `src/app/(mainLayout)/layout.js` — کامپوننت `ProtectedContent` که با `AccountContext.isAuthenticated/isLoading` کار می‌کند و به `/auth/login` ریدایرکت می‌کند. Middleware سمت سرور وجود ندارد.

## 4. Folder Structure

| Path | Purpose | Key Files |
|---|---|---|
| `src/app/` | App Router: routeها، layouts، BFF API routes، i18n | `layout.js`, `(mainLayout)/layout.js` |
| `src/app/api/auth/` | BFF route handlerها برای احراز هویت | `login/route.js`, `refresh/route.js`, `me/route.js` |
| `src/components/` | کامپوننت‌های shared | `common/PermissionGuard.js`, `reactstrapFormik/` |
| `src/elements/` | UI primitiveهای قالب (دکمه، مودال، تصویر) | `buttons/Btn.js`, `alerts&Modals/Modal.js` |
| `src/layout/` | شِل داشبورد | `index.js`, `header/`, `sidebar/`, `footer/`, `TanstackWrapper.jsx`, `ErrorBoundary.jsx` |
| `src/helper/` | Context providers (state سراسری) | `accountContext/`, `settingContext/` |
| `src/lib/` | منطق اپلیکیشنی مدرن (ترجیح پروژه) | `api/clientApi.js`, `api/serverAuth.js`, `auth/permissions.js`, `toast/` |
| `src/utils/` | یوتیلیتی‌ها + کدهای legacy قالب | `validation/`, `hooks/`, `customFunctions/`, `axiosUtils/` (legacy) |
| `public/assets/` | استایل SCSS، فونت، تصویر | `scss/app.scss`, `scss/layout/_navira-layout.scss` |
| `bot/template/` | قالب مرجع خریداری‌شده (فقط مرجع) | مستقل از build اصلی |

## 5. Application Architecture
- **Layer-based** (نه feature-based) — در حال حاضر پروژه early-stage است.
- تقریباً همه چیز **Client Component** است (`"use client"` در root layout). Server Component واقعی فقط BFF route handlerها هستند.
- Business logic: در `src/lib/` (جدید) و `src/utils/` (legacy).
- Data fetching احراز هویت: client → `apiFetch` → `/api/auth/*` (BFF) → Backend با Bearer token از httpOnly cookie.
- Data fetching داده‌های اپ: هنوز پیاده نشده؛ الگوی legacy قالب (`src/utils/axiosUtils` + هوک‌های `useCreate/useUpdate/useDelete/useCustomQuery` مبتنی بر TanStack Query) موجود اما به Backend واقعی وصل نیست.
- Formها: Formik + wrapperهای Reactstrap؛ validation با Yup در `src/utils/validation/ValidationSchemas.js`.
- Error handling: `apiFetch` خطاهای HTTP و business (`success:false`/`error:true`) را به `Error` با `status` و `data` تبدیل می‌کند؛ نمایش با `Toast.error`. `ErrorBoundary` سراسری در `src/layout/ErrorBoundary.jsx`.

## 6. UI System & Styling
- بدون Tailwind / CSS Modules / shadcn / MUI — **Bootstrap 5 + Reactstrap + SCSS کاستوم**.
- theme: متغیرها در `public/assets/scss/utils/_variables.scss` (`$primary-color: #ec8951`, `--theme-color`).
- **overrideهای اختصاصی پروژه فقط در `public/assets/scss/layout/_navira-layout.scss`** (آخرین import در `style.scss`) — استایل هدر مدرن، رفتار RTL سایدبار/هدر و دارک‌مود هدر همین‌جاست. قانون: تغییرات استایل پروژه را به این فایل اضافه کن، فایل‌های قالب را دست نزن.
- Dark mode: کلاس `dark-only` روی `<body>` (از SettingProvider)؛ overrideهای مربوطه در انتهای `_navira-layout.scss`.
- **RTL**: `dir="rtl"` و `lang="fa"` در root layout؛ wrapperها `dir="rtl"` دارند؛ toast با `rtl: true`.
- فونت: Montserrat از Google Fonts در `src/app/layout.js`. **فونت فارسی (Vazir/IRANSans) فعلی: Not found** — بدهی فنی.
- localize تاریخ/عدد/ارز فارسی: Not found.

## 7. Component Conventions
- نام‌گذاری: فایل‌ها `index.js` در پوشهٔ نام‌دار (PascalCase برای کامپوننت) یا تک‌فایل PascalCase؛ همه JSX با `"use client"`.
- بدون TypeScript — props بدون type definition.
- wrapperهای فرم مبتنی بر Formik در `src/components/reactstrapFormik/` (الگوی `<Field component={ReactstrapInput} .../>`).

| Component | Path | Type | Usage |
|---|---|---|---|
| `Layout` | `src/layout/index.js` | client | شِل داشبورد: Header + Sidebar + Footer |
| `Header` | `src/layout/header/index.js` | client | هدر مدرن: لوگو، toggle سایدبار (موبایل)، آواتار کوچک + دراپ‌داون (پروفایل/خروج) |
| `Sidebar` | `src/layout/sidebar/index.js` | client | منوی تودرتو از `MenuData.js`؛ off-canvas در موبایل |
| `Footer` | `src/layout/footer/index.js` | client | کپی‌رایت ساده |
| `Btn` | `src/elements/buttons/Btn.js` | client | دکمه استاندارد قالب |
| `ShowModal` | `src/elements/alerts&Modals/Modal.js` | client | مودال تأیید/عمومی |
| `ReactstrapFormikInput` و مشابه | `src/components/reactstrapFormik/` | client | اتصال Formik به Reactstrap |
| `PermissionGuard` | `src/components/common/PermissionGuard.js` | client | رندر شرطی بر اساس permission |
| `LoginBoxWrapper` | `src/utils/hoc/LoginBoxWrapper.js` | HOC | چیدمان باکس فرم‌های auth |
| Skeleton loader | `src/elements/posSkeletonLoader/` | client | loading skeleton |

- Barrel export: `src/components/reactstrapFormik/index.js`, `src/helper/accountContext/index.js`, `src/lib/toast/index.js`.

## 8. State Management
بدون Zustand/Redux — Context API + useReducer؛ server-state با TanStack Query.

| Store/Context | Path | Responsibility | State مهم | Actions | Persistence |
|---|---|---|---|---|---|
| `AccountContext` | `src/helper/accountContext/` | کاربر جاری، دسترسی‌ها، احراز هویت | `userInfo`, `userAccess`, `isAuthenticated`, `isLoading` | `refreshProfile()`, `logout()` | خیر (از `/api/auth/me` خوانده می‌شود) |
| `SettingContext` | `src/helper/settingContext/` | تنظیمات سایت، وضعیت سایدبار | `settingObj`, `state` (لوگو/فاوآیکون/mode)، `sidebarOpen` | `setSidebarOpen`, `dispatch` | خیر |
| React Query cache | `src/app/layout.js` | cache داده‌های API (legacy hooks) | — | — | خیر |

نکته: AccountProvider هم در root layout و هم در `(mainLayout)/layout.js` تکرار شده — بدهی فنی.

## 9. API Integration
- **Base URL**: `NEXT_PUBLIC_API_BASE_URL` (پیش‌فرض `http://localhost:5979`) — فقط سمت سرور (BFF) برای صدا زدن Backend استفاده می‌شود.
- **الگوی اصلی (BFF)**: client با `apiFetch` (`src/lib/api/clientApi.js`) به route handlerهای داخلی `/api/auth/*` می‌زند؛ آن‌ها با `callBackendWithAuth` (`src/lib/api/serverAuth.js`) توکن را از httpOnly cookie خوانده و به‌صورت `Authorization: Bearer` به Backend پاس می‌دهند.
- Token attach: سمت سرور در BFF (نه client). Interceptor: ندارد (fetch ساده).
- Refresh: `POST /api/auth/refresh` → Backend `/api/auth/refresh-token` → ست مجدد کوکی‌ها. فراخوانی خودکار refresh در client: Not found (Needs verification — باید پیاده شود).
- Error normalization: `apiFetch` خطا را به `Error{message,status,data}` تبدیل می‌کند.
- DTO/type: Not found (جاوااسکریپت ساده).

| Service | Path | Base Endpoint | Methods |
|---|---|---|---|
| BFF login | `src/app/api/auth/login/route.js` | `{BACKEND}/api/auth/login` | POST |
| BFF refresh | `src/app/api/auth/refresh/route.js` | `{BACKEND}/api/auth/refresh-token` | POST |
| BFF logout | `src/app/api/auth/logout/route.js` | — (پاک‌کردن کوکی) | POST |
| BFF me | `src/app/api/auth/me/route.js` | `{BACKEND}/api/auth/me` | GET — **در حال حاضر mock برمی‌گرداند؛ نسخه واقعی در کامنت موجود است** |
| Legacy axios client | `src/utils/axiosUtils/index.js` + `API.js` | `process.env.API_PROD_URL` (از `next.config.js`) | wrapper قدیمی قالب با کوکی `uat` — برای کدهای جدید استفاده نکن |

## 10. Authentication & Authorization
- بدون Keycloak/NextAuth — **Custom JWT** از Backend.
- login: `POST /api/auth/login` (BFF) توکن‌ها را فقط در **httpOnly cookie** (`access_token`, `refresh_token`؛ `sameSite=lax`؛ `secure` در production) ست می‌کند؛ مقدار توکن در body پاسخ نیست.
- خواندن وضعیت: `AccountProvider.refreshProfile()` → `GET /api/auth/me` → `{ userInfo, userAccess }` در context.
- logout: `POST /api/auth/logout` (پاک‌کردن کوکی‌ها) + `router.replace("/auth/login")`.
- Guard: `ProtectedContent` در `src/app/(mainLayout)/layout.js` (client-side redirect). Middleware سروری: Not found (proxy خالی).
- Permissionها: آرایه رشته‌ای `userAccess` از Backend با الگوی `Method.Controller.App` (مثل `Get.Dashboard.Admin`). helperها در `src/lib/auth/permissions.js` (`hasPermission/hasAnyPermission/hasAllPermissions`) و کامپوننت `PermissionGuard`.
- `src/utils/auth.js` (کوکی‌های `uat/urt/utt`) legacy است و با جریان فعلی استفاده نمی‌شود — برای کد جدید استفاده نکن.

## 11. Environment Variables & Configuration

| Key | Purpose | Public/Server | Required |
|---|---|---|---|
| `NEXT_PUBLIC_API_BASE_URL` | آدرس پایه Backend API (در BFF routes خوانده می‌شود) | Public name / Server use | بله |

- `.env.local` (مقدار واقعی) و `.env.local.example` موجود.
- `next.config.js`: `reactStrictMode:false`؛ `env` legacy قالب (`API_PROD_URL`, `storageURL`)؛ redirect `/`→`/dashboard`؛ `images.remotePatterns` فقط localhost.
- `vercel.json`: CORS headers برای `/api/*`.
- `jsconfig.json`: alias `@/*`.
- `src/proxy.js`: middleware خالی (غیرفعال).
- ESLint: بدون config جدا (Next built-in). Prettier / tsconfig / tailwind / postcss: Not found.

## 12. Forms & Validation

| Form | Path | Library | Validation | Submit Action |
|---|---|---|---|---|
| Login | `src/app/(authLayout)/auth/login/page.js` | Formik | Yup (`YupObject`, `nameSchema`, `passwordSchema`) | `apiFetch("/api/auth/login")` + `refreshProfile()` + redirect |
| Forgot password | `src/app/(authLayout)/auth/forgot-password/page.js` | Formik | Yup | BFF `/api/auth/forgot-password` |

- اسکیماها: `src/utils/validation/ValidationSchemas.js` (پیام‌های خطا هنوز انگلیسی — بدهی فنی).
- نمایش خطا: `<ErrorMessage>` از Formik.
- reCAPTCHA در login: `react-google-recaptcha` با فلگ `settingObj?.google_reCaptcha?.status`.

## 13. Data Fetching & Caching
- **الگوی فعلی (auth)**: client → `apiFetch` → BFF route handler → Backend. ساده و امن؛ بدون cache (`cache:"no-store"`).
- **الگوی legacy قالب**: `src/utils/hooks/useCustomQuery` + `useCreate/useUpdate/useDelete` (TanStack Query + axios wrapper). برای داده‌های اپ در آینده قابل استفاده است، اما ابتدا باید به الگوی BFF سازگار شود (فعلاً به کوکی `uat` قدیمی تکیه دارد — Needs verification).
- Server Components data fetching / server actions / revalidate: Not found.
- Convention پیشنهادی پروژه برای آینده: BFF route در `src/app/api/<resource>/route.js` + هوک TanStack Query که همان مسیر داخلی را صدا می‌زند.

## 14. Internationalization & Localization
- i18next + react-i18next نصب؛ تنظیمات در `src/app/i18n/settings.js` (fallback `en`؛ زبان‌ها: en/ar/fr/es — **`fa` در لیست `languages` نیست** ولی `locales/fa/common.json` وجود دارد؛ باگ).
- فایل‌های ترجمه: `src/app/i18n/locales/{ar,en,es,fa,fr}/common.json` (اکثراً باقی‌مانده قالب).
- **وضعیت واقعی**: UI فارسیِ hardcode؛ متن‌ها به‌صورت object محلی در هر صفحه (مثل `loginTexts`, `dashboardTexts`). i18next عملاً در صفحات اصلی استفاده نمی‌شود.
- Convention فعلی پروژه: متن فارسی مستقیم در کامپوننت (object متنی در ابتدای فایل) — تا تصمیم نهایی درباره i18n.
- RTL: کامل (dir=rtl سراسری). Date/number/currency فارسی: Not found. Language switcher: حذف شده.

## 15. Error Handling & Notifications
- Toast: `src/lib/toast/index.js` (`Toast.success/error/warning/info/show/dismiss`) + `ToastProvider` در root layout (rtl، top-center، theme=colored).
- `react-toastify` در SCSS هم import شده.
- Global error boundary: `src/layout/ErrorBoundary.jsx` (در root layout فعال است).
- `app/error.js` / `not-found.js`: Not found.
- خطاهای API: `apiFetch` throw می‌کند؛ caller با try/catch و `Toast.error(error.message)` نمایش می‌دهد.
- Logging: Not found.

## 16. Security Notes
- **Token storage**: httpOnly cookie سمت سرور (الگوی درست؛ مقاوم در برابر XSS token theft). توکن در body پاسخ login برنمی‌گردد.
- کوکی‌های legacy قالب (`uat/urt/utt`، `js-cookie`، localStorage `account`) قابل خواندن توسط JS هستند — فقط در کدهای legacy؛ نباید گسترش یابد.
- Protected routes فقط client-side هستند (قابل دور زدن برای محتوای حساس؛ middleware سروری توصیه می‌شود — بدهی فنی).
- CSRF: `sameSite=lax` روی کوکی‌ها؛ BFFها body JSON می‌پذیرند.
- `NEXT_PUBLIC_API_BASE_URL` به client expose می‌شود ولی فقط سرور از آن استفاده می‌کند.
- CSP / security headers: Not found (فقط CORS در `vercel.json`).

## 17. Performance & Optimization
- `next/image` برای لوگو و آواتار استفاده می‌شود؛ `remotePatterns` باید با دامنه production به‌روز شود.
- Font: Google Fonts با `<link>` در root layout (نه `next/font`) — قابل بهینه‌سازی.
- Code splitting/dynamic import: صفحات App Router پیش‌فرض؛ dynamic import دستی: Not found.
- Memoization: `useMemo/useCallback` در `AccountProvider`.
- Bundle analyzer / virtualization: Not found.
- Server/Client balance: تقریباً همه client است — فرصت بهینه‌سازی در آینده.

## 18. Testing
Not found — بدون Jest/Vitest/RTL/Playwright/Cypress/Storybook. بدون اسکریپت test.

## 19. Build, Run & Deployment

| Task | Command |
|---|---|
| Install | `npm install` |
| Dev | `npm run dev` |
| Build | `npm run build` |
| Start | `npm run start` |
| Lint | `npm run lint` |
| Test / typecheck / format | Unknown (ندارد) |

- Deployment target: Vercel (`vercel.json`). Output mode: پیش‌فرض. Dockerfile: Not found. CI/CD: Not found.
- Build با Turbopack؛ static pages: `/`, `/dashboard`, `/profile`, `/page`, `/auth/*`؛ dynamic: `/api/auth/*`.

## 20. How To Add a New Page
1. پوشهٔ route در `src/app/(mainLayout)/<name>/` بساز و `page.js` با `"use client"` داخلش قرار بده.
2. اگر صفحه باید در شِل داشبورد باشد، همین route group کافی است (گارد + Layout خودکار اعمال می‌شود).
3. متن‌های فارسی را در یک object (مثل `myTexts`) بالای فایل صفحه تعریف کن.
4. از کامپوننت‌های موجود (`Btn`, `Card` از reactstrap، `PermissionGuard`) استفاده کن.
5. استایل اختصاصی لازم بود، به `public/assets/scss/layout/_navira-layout.scss` اضافه کن.
6. آیتم منو در `src/layout/sidebar/MenuData.js` اضافه کن (آیکون از `react-icons/ri`).
7. اگر permission لازم است، `PermissionGuard` یا چک با `userAccess` اضافه کن.

## 21. How To Add a New Feature
1. route صفحات feature در `src/app/(mainLayout)/<feature>/` (لیست، create، `edit/[updateId]`).
2. کامپوننت‌های feature را در `src/components/<feature>/` بساز.
3. BFF route در `src/app/api/<feature>/route.js` که با `callBackendWithAuth` به Backend وصل می‌شود.
4. هوک data fetching با TanStack Query که مسیر داخلی BFF را صدا می‌زند (در `src/utils/hooks/` یا کنار feature).
5. فرم‌ها با Formik + Yup؛ اسکیما در `src/utils/validation/ValidationSchemas.js` (پیام‌های فارسی).
6. آیتم منو + permission check + loading/error/empty state با `Toast`.

## 22. How To Add a New API Service
1. فایل `src/app/api/<resource>/route.js` بساز (GET/POST/...).
2. داخلش با `callBackendWithAuth("/api/<resource>", {...})` از `src/lib/api/serverAuth.js` به Backend بزن؛ توکن خودکار از httpOnly cookie attach می‌شود.
3. پاسخ را با `NextResponse.json({ success, data })` نرمال کن؛ خطاها با status مناسب.
4. سمت client از `apiFetch` (`src/lib/api/clientApi.js`) استفاده کن — خطاها به `Error{message,status,data}` تبدیل می‌شوند.
5. از axios wrapper legacy (`src/utils/axiosUtils`) برای کد جدید استفاده نکن.
6. نام‌گذاری methodها: RESTful بر اساس route handlerهای Next.

## 23. How To Add a Protected Route
1. صفحه را داخل `src/app/(mainLayout)/` بگذار — گارد auth (`ProtectedContent`) خودکار اعمال می‌شود و به `/auth/login` ریدایرکت می‌کند.
2. برای سطح دسترسی دقیق‌تر: `PermissionGuard` (`src/components/common/PermissionGuard.js`) با propهای `permission`/`anyOf`/`allOf` دور محتوا بزن.
3. loading state: گارد موجود `isLoading` را با متن ساده نشان می‌دهد (قابل بهبود با skeleton).
4. Unauthorized page اختصاصی: Not found (فعلاً fallback=null).

## 24. Permission & Menu Visibility
- Permissionها از Backend می‌آیند: `GET /api/auth/me` → `userAccess: string[]` در `AccountContext`.
- **نامی‌گذاری**: الگوی `Method.Controller.App` — مثال واقعی: `Get.Dashboard.Admin`.
- helperها: `src/lib/auth/permissions.js` + `PermissionGuard`.
- **منوها فعلاً بر اساس permission فیلتر نمی‌شوند** — `MenuData.js` استاتیک است (بدهی فنی؛ باید permission به آیتم‌ها اضافه و در `Sidebar` فیلتر شود).
- وضعیت منو (2026-07-27): منوهای قالب پاک شدند؛ فقط یک منوی «احراز هویت» با زیرمنوی «دسترسی‌ها» (`/access`) وجود دارد.
- route guard بر اساس permission: فقط `PermissionGuard` در سطح کامپوننت؛ guard سطح route: Not found.

### مدیریت دسترسی‌ها (Access Management)
- صفحه: `src/app/(mainLayout)/access/page.js` — سه تب به ترتیب: نقش‌ها (Role)، سیاست‌ها (Policy)، مجوزها (Permission).
- کامپوننت‌ها: `src/components/access/{PolicyList,RoleList,PermissionList,ConnectionsModal,PolicyFormModal,accessUtils}.js` — `PolicyFormModal` فرم ایجاد سیاست (name + description) با `POST /api/access/policies`.
- جدول مشترک: `src/components/common/NaviraDataTable.js` (جستجو + pagination سمت client؛ در موبایل ردیف‌ها به کارت تبدیل می‌شوند).
- هوک‌ها: `src/utils/hooks/access/useAccessCrud.js` (`useAccessList`, `useAccessAssign`) — با `apiFetch` به `/api/access/*` (الگوی BFF).
- مدل اتصال: **Role چند Policy دارد** (`role.policyIds: []`) و **Policy چند Permission دارد** (`policy.permissions: string[]` با الگوی `Method.Controller.App`). مجوزهای یک نقش = اجتماع مجوزهای سیاست‌هایش.
- اتصال‌ها: اتصال سیاست‌ها به نقش (`PUT /api/access/roles/{id}` با `{ policies: [] }`) و اتصال مجوزها به سیاست (`PUT /api/access/policies/{id}` با `{ permissions: [] }`) از طریق `ConnectionsModal`.
- استایل: `public/assets/scss/pages/_access.scss` (در `style.scss` ثبت شده).

## 25. Important Files & Components

| Name | Path | Responsibility |
|---|---|---|
| Root layout | `src/app/layout.js` | html rtl/fa، providers، Toast، fonts |
| Main layout/guard | `src/app/(mainLayout)/layout.js` | auth guard + شِل داشبورد |
| Header | `src/layout/header/index.js` | هدر مدرن + منوی پروفایل |
| Sidebar | `src/layout/sidebar/index.js` + `MenuData.js` | ناوبری اصلی |
| AccountProvider | `src/helper/accountContext/AccountProvider.js` | وضعیت کاربر + login/logout |
| apiFetch | `src/lib/api/clientApi.js` | fetch client→BFF با normalize خطا |
| serverAuth | `src/lib/api/serverAuth.js` | BFF→Backend با Bearer از کوکی |
| BFF auth routes | `src/app/api/auth/*/route.js` | login/refresh/logout/me |
| permissions | `src/lib/auth/permissions.js` | hasPermission و مشتقات |
| PermissionGuard | `src/components/common/PermissionGuard.js` | رندر شرطی بر اساس دسترسی |
| Toast | `src/lib/toast/` | اعلان فارسی RTL |
| Navira SCSS overrides | `public/assets/scss/layout/_navira-layout.scss` | استایل‌های اختصاصی پروژه |
| Validation schemas | `src/utils/validation/ValidationSchemas.js` | اسکیمای Yup |
| Template مرجع | `bot/template/` | قالب خریداری‌شده (فقط مرجع) |

## 26. Common Developer Tasks
- اجرا: `npm run dev` | build: `npm run build` | lint: `npm run lint`
- صفحه جدید: بخش ۲۰ | feature: بخش ۲۱ | API service: بخش ۲۲ | route محافظت‌شده: بخش ۲۳
- منو: ویرایش `src/layout/sidebar/MenuData.js`
- env variable: `.env.local` + ثبت کلید در بخش ۱۱ این سند
- ترجمه: فعلاً متن فارسی hardcode در صفحه (بخش ۱۴)
- تست/typecheck/format: Unknown (ابزار ندارد)
- deploy: Vercel

## 27. AI Assistant Instructions
- قبل از هر تحلیل، ابتدا همین فایل را بخوان؛ اگر پاسخ در آن هست، کل پروژه را اسکن نکن.
- فقط فایل‌های مرتبط با task را باز کن.
- task مربوط به route → بخش ۳ و ۲۰ | auth → بخش ۱۰ و ۲۳ | API → بخش ۹ و ۲۲ | UI/استایل → بخش ۶ و ۷ | permission → بخش ۲۴.
- معماری پروژه: BFF + httpOnly cookie. از الگوی legacy `src/utils/axiosUtils` و کوکی `uat` برای کد جدید استفاده نکن.
- استایل‌های جدید فقط در `_navira-layout.scss`؛ فایل‌های SCSS قالب را override نکن.
- UI فارسی و RTL است؛ متن‌ها را فارسی و hardcode (object متنی بالای فایل) نگه دار تا تکلیف i18n مشخص شود.
- dependency جدید بدون دلیل واضح اضافه نکن؛ پیش از تغییر `next.config.js`، `src/proxy.js`، BFF auth routes یا `clientApi/serverAuth` دلیل را توضیح بده.
- `bot/template/` فقط مرجع است؛ منطق/بیزینس آن را کپی نکن.
- اگر معماری یا convention تغییر کرد، همین سند را در همان بخش مرتبط به‌روزرسانی کن.
- اطلاعات حساس (token, secret, مقدار واقعی env) را در هیچ خروجی ننویس.

## 28. Known Risks / Technical Debt
- `src/app/api/auth/me/route.js` پاسخ **mock** برمی‌گرداند؛ نسخه واقعی در کامنت است — باید با Backend وصل شود.
- `src/app/api/access/{policies,roles,permissions}/route.js` فعلاً **داده دمو** برمی‌گردانند؛ نسخه واقعی (callBackendWithAuth) در کامنت است و باید با Backend وصل شود.
- گارد auth فقط client-side است؛ `src/proxy.js` (middleware) خالی و غیرفعال.
- `AccountProvider` دو بار (root + mainLayout) mount می‌شود → دو درخواست `/api/auth/me`.
- دو سیستم موازی auth/API: جدید (`src/lib`, کوکی `access_token`) و legacy (`src/utils/axiosUtils`، کوکی `uat`) — legacy باید حذف/مهاجرت شود.
- فونت فارسی و localize تاریخ/عدد: Not found.
- منوهای سایدبار بر اساس permission فیلتر نمی‌شوند.
- i18n نیمه‌کاره: `fa` در لیست languages نیست؛ `languages` در `settings.js` یک آیتم خالی دارد (`, ,`).
- پیام‌های validation انگلیسی‌اند.
- `src/app/(mainLayout)/page/page.js` placeholder تستی ("Salam").
- refresh token خودکار در client پیاده نشده.
- بدون تست، Prettier، CSP.
- استفاده از `&` در نام پوشه‌ها (`elements/alerts&Modals`, `q&a` در قالب) می‌تواند در برخی ابزارها دردسرساز شود.

## 29. Open Questions
- قرار است i18next فعال و چندزبانه شود یا فارسیِ hardcode به‌عنوان convention نهایی بماند؟
- منوی `MenuData.js` کدام بخش‌ها (users/products/orders/...) واقعاً در بیزینس Navira لازم‌اند؟
- آیا routeهای data (غیر auth) هم از الگوی BFF پیروی می‌کنند یا client مستقیم به Backend می‌زند؟
- نام دقیق claim/permissionها و قرارداد پاسخ `me` از Backend چیست؟
- فونت فارسی نهایی کدام است (Vazirmattan/IRANSans)؟
- آیا middleware سروری برای guard جایگزین گارد client شود؟
- هدف deploy فقط Vercel است یا Docker هم لازم است؟

## 30. Last Updated
- Date: 2026-07-27
- Generated by: GapCode AI Assistant (coding agent)
- File: `AI_FRONTEND_CONTEXT.md` (project root)

## Policy Tab Integration (Added on 2026-08-18)

- Policy GET integration: Connected to backend GET /api/Policy endpoint via BFF route in `src/app/api/access/policies/route.js`
- Policy POST integration: Connected to backend POST /api/Policy endpoint via same BFF route
- Files modified: `src/app/api/access/policies/route.js`, `src/components/access/PolicyList.js`
- API contract: Uses PolicyDto {id, name, title, description, isSystem, isActive} from backend
- Authentication approach: Reused existing Bearer token flow from httpOnly cookies via `callBackendWithAuth`
- Policy-list refresh strategy: Uses existing `queryClient.invalidateQueries({ queryKey: [\"access\"] })` pattern after successful creation
- Validation and error-handling: Reused existing patterns with error states and Persian messages

Files changed or created:
- `src/app/api/access/policies/route.js`: Updated to connect to real backend API instead of demo data
- `src/components/access/PolicyList.js`: Added error handling similar to RoleList component


## Policy Create Flow Update (2026-08-19)
- Policy creation: `PolicyFormModal` (`src/components/access/PolicyFormModal.js`) now submits the exact backend contract `CreatePolicyRequest { name, title, description, isSystem, isActive }` (blank strings → `null`; booleans default `true`).
- BFF POST `src/app/api/access/policies/route.js` sends `Content-Type: application/json-patch+json` to `POST /api/Policy` with Bearer token from httpOnly cookie via `callBackendWithAuth`.
- Backend `PolicyController` requires Bearer auth (`[CustomAuthorize]`); unauthenticated requests return 401 propagated as `{success:false}`.
- Failure keeps modal open with entered values; error shown via existing `Toast.error` in `useAccessCreate`.
- Success: existing success toast, modal closes, form resets, `queryClient.invalidateQueries({ queryKey: ["access"] })` refreshes the list.
- GET `/api/Policy` list integration unchanged (already live from previous task); PolicyList has error/loading/empty states.

## Permission Tab Integration (2026-08-22)
- Permission API: GET `/api/Permission` via BFF `src/app/api/access/permissions/route.js` (demo data removed), Bearer token from httpOnly cookie via `callBackendWithAuth`, `Content-Type: application/json`, `{ error, message, data }` envelope unwrapped (error:true treated as failure).
- Permission DTO fields: `id, baseSubSystemId, controllerName, scope, code, title, isActive` (JSDoc typedef in the BFF route).
- `src/components/access/PermissionList.js` rebuilt: NaviraDataTable columns شناسه/شناسه زیرسیستم پایه/کنترلر/محدوده دسترسی/کد دسترسی/عنوان/وضعیت; isActive → Badge success(فعال)/danger(غیرفعال); loading/empty/error states; error state mirrors RoleList/PolicyList pattern (`navira-table-state text-danger` + RiErrorWarningLine).
- Fetching: existing `useAccessList("permissions")` TanStack Query pattern; fetch on tab activation, cached (no duplicate requests).

## Centralized 401/403 Handling (2026-08-22)
- 401 Handling: `apiFetch` in `src/lib/api/clientApi.js` intercepts HTTP 401 responses (except for auth endpoints), triggers token refresh via `/api/auth/refresh`, uses shared refresh lock (`isRefreshing`) to prevent multiple refresh requests, queues pending requests with `refreshSubscribers`, retries original requests with new tokens after successful refresh.
- 401 Failure: When refresh fails, clears auth credentials via "auth:logout" event dispatched to `AccountProvider`, redirects to `/auth/login`.
- 403 Handling: `apiFetch` intercepts HTTP 403 responses, redirects to `/403` page without clearing auth credentials or triggering refresh.
- 403 Page: Created `src/app/403/page.js` - Persian 403 forbidden page with countdown timer that redirects to home after 10 seconds.
- Route Guard: `src/app/(mainLayout)/layout.js` continues to protect routes; `AccountProvider` listens for "auth:logout" events to update auth state.
- Refresh Lock: Shared state prevents parallel refresh requests; subsequent 401s during refresh wait for the same refresh operation.

## Role-Policy Assignment (2026-08-23)
- BFF route `src/app/api/access/role-policies/route.js`: GET proxies `GET /api/RolePolicy/{roleId}` (extracts assigned policy IDs from `{data}` envelope, supports ID or object items), POST proxies `POST /api/RolePolicy` with `Content-Type: application/json-patch+json` and exact backend payload `{ roleId, policyAsinge, policyUnAsinge }` (backend spelling preserved). Auth: Bearer from httpOnly cookie via `callBackendWithAuth`.
- Hooks in `src/utils/hooks/access/useAccessCrud.js`: `useRolePolicies(roleId)` (TanStack Query, cached per role, no duplicate fetches) and `useSaveRolePolicies()` (success toast + invalidates `["access","role-policies"]` and `["access","roles"]`).
- `RoleList`: opens ConnectionsModal per role; baseline = assigned IDs from `useRolePolicies`; on save computes Set difference (policyAsinge = selected − assigned, policyUnAsinge = assigned − selected); no-op when unchanged (closes without POST); failure keeps modal open via existing toast error handling.
- `ConnectionsModal`: added client-side search (name/title/code/description, case-insensitive, Persian-safe, selection preserved while filtering); save/cancel disabled while saving.
