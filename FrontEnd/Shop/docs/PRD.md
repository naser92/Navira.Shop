# PRD - NaviraShop Mug Store Frontend

## 1. Overview

این پروژه یک وب‌سایت فروشگاهی فارسی برای فروش ماگ است. پنل ادمین جداگانه وجود دارد و این پروژه فقط storefront عمومی، حساب کاربری، سبد خرید و فرایند خرید را پوشش می‌دهد.

Backend با ASP.NET 9 پیاده‌سازی شده و Frontend از طریق REST API به آن متصل می‌شود.

## 2. Goals

- ساخت فروشگاه فارسی RTL
- طراحی responsive برای موبایل، تبلت و دسکتاپ
- استفاده از Next.js App Router
- رعایت معماری component-based و framework-based
- پرهیز از کد تکراری
- استفاده صحیح از Server Components و Client Components
- اتصال استاندارد به APIهای ASP.NET 9
- طراحی glassmorphism شبیه iOS
- حفظ SEO و performance

## 3. Tech Stack

- Next.js 15+
- TypeScript
- React 19+
- Sass / SCSS Modules
- Redux Toolkit
- Redux Persist
- TanStack Query
- Axios یا Fetch Wrapper
- Zod برای validation
- React Hook Form برای فرم‌ها
- Next Metadata API برای SEO

## 4. Language And Direction

- زبان سایت فارسی است.
- کل سایت باید RTL باشد.
- html باید `lang="fa"` و `dir="rtl"` داشته باشد.
- در CSS باید از logical properties مثل `margin-inline`, `padding-inline`, `inset-inline` استفاده شود.
- استفاده مستقیم از `left` و `right` فقط در موارد ضروری مجاز است.

## 5. Architecture Rules

- پروژه باید feature-based باشد.
- کامپوننت‌های عمومی در `framework/ui` قرار بگیرند.
- layoutهای عمومی در `framework/layout` قرار بگیرند.
- منطق هر دامنه در `features/{feature-name}` قرار بگیرد.
- API client مرکزی در `core/api` قرار بگیرد.
- تایپ‌های مشترک در `core/types` قرار بگیرند.
- state سراسری در `store` و state هر feature در همان feature مدیریت شود.
- هیچ API call مستقیمی داخل JSX صفحه نوشته نشود.
- هیچ style تکراری برای glass، container، grid و button نوشته نشود.

## 6. Server/Client Rules

- صفحات SEO محور باید Server Component باشند.
- کامپوننت‌ها فقط در صورت نیاز `use client` داشته باشند.
- event handler، state، localStorage، Redux hooks و browser API فقط در Client Component مجاز است.
- دریافت اولیه محصولات، دسته‌بندی‌ها و جزئیات محصول باید تا حد امکان server-side انجام شود.
- سبد خرید، wishlist، drawerها و فیلترهای تعاملی client-side هستند.

## 7. Styling Rules

- Sass Modules برای کامپوننت‌ها استفاده شود.
- متغیرها، breakpointها، mixinها، glass effect و typography در `src/styles` تعریف شوند.
- کامپوننت‌ها نباید رنگ، shadow، radius و spacing دلخواه تکراری داشته باشند.
- طراحی باید mobile-first باشد.
- تمام کامپوننت‌ها باید در عرض‌های 320px تا دسکتاپ بزرگ تست شوند.
- متن داخل دکمه‌ها و کارت‌ها نباید در موبایل overflow شود.

## 8. Design System

Theme: iOS-like glassmorphism

- سطوح شیشه‌ای با blur کنترل‌شده
- رنگ‌های pastel
- border نرم
- shadow سبک
- radius استاندارد 12px تا 20px
- تمرکز اصلی روی تصویر محصول
- کارت محصول باید خوانا، سریع و قابل اسکن باشد

## 9. Main Pages

- Home
- Product Listing
- Product Details
- Category Page
- Search Results
- Cart
- Checkout
- Login/Register
- Profile
- Orders
- About
- Contact
- Terms

## 10. Main Features

- Product catalog
- Category navigation
- Product filtering
- Product sorting
- Product search
- Product detail gallery
- Add to cart
- Cart drawer
- Checkout flow
- User authentication
- Order history
- Wishlist
- Responsive mobile navigation

## 11. API Integration

- Backend: ASP.NET 9 REST API
- تمام requestها باید از wrapper مرکزی عبور کنند.
- Error handling مرکزی لازم است.
- Token handling در لایه auth انجام شود.
- Pagination، sorting و filtering باید تایپ‌شده باشند.
- response مدل‌ها باید TypeScript interface داشته باشند.

## 12. Documentation Rules

برای هر feature باید فایل README.md وجود داشته باشد که شامل موارد زیر باشد:

- هدف feature
- فایل‌های اصلی
- APIهای مصرفی
- stateهای مربوطه
- کامپوننت‌های عمومی استفاده‌شده
- نکات SSR/CSR

## 13. AI Coding Rules

هوش مصنوعی باید قبل از ساخت هر بخش:
- معماری پروژه را بررسی کند.
- از کامپوننت‌های موجود استفاده کند.
- کامپوننت تکراری نسازد.
- از design tokens استفاده کند.
- بدون نیاز `use client` اضافه نکند.
- API call مستقیم داخل component ننویسد.
- تمام خروجی‌ها را با RTL و responsive بودن بسازد.
