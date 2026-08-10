# NaviraShop Architecture Guidelines

## 1. فلسفه معماری
این پروژه از رویکرد **Feature-based Modular Architecture** پیروی می‌کند. هدف، تفکیک کامل مسئولیت‌ها (Separation of Concerns)، قابلیت نگهداری بالا و جلوگیری از تکرار کد (DRY) است.

## 2. ساختار پوشه‌بندی (Folder Structure)
```text
src/
├── app/              # Next.js App Router (Pages, Layouts, Metadata)
├── core/             # لایه زیرساخت (Config, API, Types, Utils, Constants)
├── framework/        # لایه پایه و طراحی (UI Components, Global Layouts, Providers)
├── features/         # لایه بیزینس (هر فیچر شامل منطق، کامپوننت‌ها، هوک‌ها و APIهای مربوطه)
├── store/            # تنظیمات مرکزی Redux و مدیریت استیت‌های سراسری
└── styles/           # فایل‌های Scss گلوبال (Variables, Mixins, Glass effects)

## 3. قوانین لایه‌ها

### 3.1. لایه `core/`
*   تمام درخواست‌های API باید از `core/api` عبور کنند.
*   تایپ‌های مشترک (DTOهای دریافتی از ASP.NET) در `core/types` تعریف می‌شوند.
*   هیچ منطق UI نباید در این لایه وجود داشته باشد.

### 3.2. لایه `framework/`
*   **framework/ui:** کامپوننت‌های پایه (Button, Input, Card, Modal). این‌ها نباید بیزینس لاجیک داشته باشند (Dumb Components).
*   **framework/layout:** اجزای ساختاری (Header, Footer, PageContainer).
*   تمام کامپوننت‌های این لایه باید `RTL-Aware` باشند.

### 3.3. لایه `features/`
*   هر فیچر (مثلاً `cart`, `product`, `auth`) یک ساختار داخلی مشابه دارد:
*   `api/`: فراخوانی‌های خاص آن فیچر.
*   `components/`: کامپوننت‌های مختص آن بخش (مثلاً `ProductCard`).
*   `hooks/`: هوک‌های بیزینس لاجیک.
*   `types/`: تایپ‌های خاص فیچر.
*   **قانون طلایی:** کامپوننت‌های داخل یک فیچر نباید مستقیماً با فیچرهای دیگر ترکیب شوند مگر از طریق `Public API` (فایل `index.ts` هر فیچر).

## 4. مدیریت درخواست‌ها (Data Fetching)
*   **Server Side:** استفاده از `Server Components` در لایه `app/` برای SEO و دریافت اولیه داده‌ها (توسط `TanStack Query` یا `fetch` مستقیم).
*   **Client Side:** استفاده از `TanStack Query` برای تمامی درخواست‌های کلاینتی (جهت Caching و State Management).
*   **Axios:** تمام درخواست‌ها باید از یک `Axios Instance` مرکزی که دارای Interceptor برای تزریق Token و Error Handling است استفاده کنند.

## 5. مدیریت استیت (State Management)
*   **Redux Toolkit:** برای داده‌های گلوبال و پیچیده (مانند وضعیت سبد خرید، پروفایل کاربر، تم).
*   **Local State:** برای مدیریت وضعیت‌های کوچک کامپوننت (مانند باز/بسته بودن یک منو) استفاده از `useState` یا `useReducer` بلامانع است.
*   **Redux Persist:** جهت حفظ سبد خرید در LocalStorage.

## 6. استانداردهای استایل (Styling)
*   استفاده از **SCSS Modules** برای تمام کامپوننت‌ها.
*   **Design System:** تمام استایل‌های تکراری باید در `styles/_tokens.scss` یا `styles/_glass.scss` تعریف شوند.
*   **RTL:** اجباری. استفاده از `margin-inline-start/end` به جای `margin-left/right`.
*   **Glassmorphism:** برای ایجاد افکت شیشه‌ای iOS، از کلاس `.glass-panel` استفاده شود (تعریف شده در `_glass.scss`).

## 7. قانون Server vs Client
*   پیش‌فرض: **Server Component**.
*   استفاده از `"use client"` فقط در موارد زیر مجاز است:
*   نیاز به `useState`, `useEffect`, `useContext`, `Redux hooks`.
*   نیاز به Browser API (مانند `window`, `localStorage`).
*   تعامل با کاربر (Event Handling).

## 8. اصول توسعه برای هوش مصنوعی
1.  **قرارداد (Contract):** قبل از ساخت هر صفحه یا کامپوننت جدید، فایل `ARCHITECTURE.md` را بررسی کن.
2.  **عدم تکرار:** اگر کامپوننتی در `framework/ui` وجود دارد، نباید در `features/` مشابه آن را بسازی.
3.  **تایپ‌دهی:** تمام پراپ‌ها (Props) باید در یک Interface مشخص تایپ‌دهی شوند.
4.  **نام‌گذاری:** نام‌گذاری فایل‌ها به صورت `PascalCase` برای کامپوننت‌ها و `kebab-case` برای فولدرها و فایل‌های غیر کامپوننت.


---

### چگونه از این فایل استفاده کنید؟

۱. این فایل را در مسیر `docs/ARCHITECTURE.md` ذخیره کنید.
۲. هر بار که چت جدیدی را شروع می‌کنید، این فایل را آپلود کنید یا به آن ارجاع دهید:
> "من از معماری تعریف شده در `docs/ARCHITECTURE.md` استفاده می‌کنم. تمام کدهای بعدی باید بر اساس این ساختار باشند."

آیا می‌خواهید برویم سراغ تعریف **`styles/_glass.scss`** (برای شروع پیاده‌سازی تم شیشه‌ای) یا ترجیح می‌دهید ابتدا ساختار `core/api` (ارتباط با ASP.NET) را بچینیم؟