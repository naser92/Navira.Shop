# Navira Admin Dashboard

پنل مدیریت فروشگاه **نویرا** — یک داشبورد ادمین مدرن، تمام‌فارسی و راست‌به‌چپ (RTL) که با [Next.js](https://nextjs.org/) (App Router) ساخته شده است.

An admin dashboard for the Navira shop — a modern, fully-Persian, RTL panel built with Next.js (App Router).

---

## ویژگی‌ها | Features

- **رابط کاربری فارسی و RTL** — تمام صفحات، منوها و پیام‌ها به زبان فارسی و راست‌به‌چپ.
- **احراز هویت امن** — ورود با `accessToken` و `refreshToken` که به‌صورت **httpOnly cookie** ذخیره می‌شوند (در دسترس جاوااسکریپت نیستند و در برابر XSS امن‌اند).
- **مدیریت وضعیت کاربر** — `AccountContext` برای اطلاعات کاربر، سطح دسترسی و خروج.
- **اعلان‌های Toast مدرن** — سیستم پیام فارسی RTL با انواع `success` / `error` / `warning` / `info`.
- **طراحی واکنش‌گرا (Responsive)** — سایدبار ثابت در دسکتاپ و منوی کشویی (off-canvas) با پس‌زمینه‌ی تیره در موبایل.
- **فرم‌سازی قدرتمند** — Formik + Yup برای اعتبارسنجی فرم‌ها.

---

## شروع به کار | Getting Started

### پیش‌نیازها | Prerequisites

- Node.js 18.18 یا بالاتر
- npm / yarn / pnpm

### نصب و اجرا | Install & Run

```bash
# نصب وابستگی‌ها
npm install

# اجرای محیط توسعه
npm run dev
```

سپس مرورگر را روی [http://localhost:3000](http://localhost:3000) باز کنید.

### اسکریپت‌ها | Scripts

| دستور | توضیح |
| ------ | ------ |
| `npm run dev` | اجرای سرور توسعه |
| `npm run build` | ساخت نسخه‌ی production |
| `npm run start` | اجرای نسخه‌ی production |
| `npm run lint` | بررسی کد با ESLint |

---

## پیکربندی | Configuration

یک فایل `.env.local` در ریشه‌ی پروژه بسازید و آدرس بک‌اند را تنظیم کنید:

```env
NEXT_PUBLIC_API_BASE_URL=http://localhost:5979
```

این مقدار به‌عنوان آدرس پایه‌ی API برای تمام درخواست‌ها (ورود، تازه‌سازی توکن، اطلاعات کاربر و …) استفاده می‌شود.

---

## ساختار پروژه | Project Structure

```
src/
├── app/
│   ├── (authLayout)/           # صفحات بدون سایدبار
│   │   └── auth/
│   │       ├── login/          # صفحه‌ی ورود
│   │       └── forgot-password/# بازیابی رمز عبور
│   ├── (mainLayout)/           # صفحات داخل داشبورد (با سایدبار)
│   │   ├── dashboard/          # صفحه‌ی خوش‌آمدگویی
│   │   └── layout.js           # گارد احراز هویت + چیدمان داشبورد
│   ├── api/auth/               # مسیرهای API سمت سرور (BFF)
│   │   ├── login/              # ذخیره‌ی توکن‌ها در httpOnly cookie
│   │   ├── refresh/            # تازه‌سازی توکن
│   │   ├── logout/             # پاک کردن کوکی‌ها
│   │   ├── me/                 # اطلاعات کاربر جاری
│   │   └── forgot-password/
│   └── layout.js               # چیدمان ریشه (dir="rtl" lang="fa" + Provider ها)
├── layout/
│   ├── sidebar/                # منوی فارسی + داده‌های منو
│   ├── header/                 # هدر با نام کاربر و خروج
│   └── footer/
├── helper/
│   ├── accountContext/         # وضعیت کاربر و احراز هویت
│   └── settingContext/         # تنظیمات کلی
└── lib/
    ├── api/                    # کلاینت API (apiFetch) + احراز هویت سرور
    ├── auth/                   # بررسی دسترسی‌ها (permissions)
    └── toast/                  # سیستم اعلان Toast فارسی RTL
```

---

## احراز هویت و امنیت توکن | Auth & Token Security

جریان ورود به‌صورت **Backend-for-Frontend (BFF)** پیاده‌سازی شده است:

1. کاربر در `auth/login` نام‌کاربری و رمز را وارد می‌کند.
2. مسیر `app/api/auth/login` درخواست را به بک‌اند می‌فرستد و پاسخ (`accessToken` / `refreshToken`) را دریافت می‌کند.
3. توکن‌ها فقط به‌صورت **httpOnly cookie** (`secure` در production و `sameSite=lax`) برای مرورگر ارسال می‌شوند و **هرگز** در بدنه‌ی JSON قرار نمی‌گیرند — بنابراین جاوااسکریپت سمت کلاینت به آن‌ها دسترسی ندارد.
4. `AccountContext` با فراخوانی `api/auth/me` اطلاعات کاربر را می‌خواند و وضعیت ورود را مدیریت می‌کند.
5. چیدمان `(mainLayout)` از صفحات محافظت می‌کند و کاربر احرازهویت‌نشده را به صفحه‌ی ورود هدایت می‌کند.

> نکته‌ی امنیتی: توکن‌ها به‌هیچ‌وجه در `localStorage` یا state جاوااسکریپت نگه‌داری نمی‌شوند.

---

## سیستم اعلان (Toast) | Toast Notifications

یک API ساده و فارسی برای نمایش پیام‌ها:

```js
import Toast from "@/lib/toast";

Toast.success("ورود با موفقیت انجام شد");
Toast.error("نام کاربری یا رمز عبور اشتباه است");
Toast.warning("هشدار نمونه");
Toast.info("اطلاعات نمونه");

// یا با مشخص کردن نوع:
Toast.show("warn", "پیام هشدار"); // success | error | warning | info
```

- راست‌به‌چپ، آیکون در سمت راست و دکمه‌ی بستن در سمت چپ.
- رنگ‌بندی مدرن (سبز/قرمز/کهربایی/آبی) با افکت blur و انیمیشن ورود.
- در موبایل تمام‌عرض نمایش داده می‌شود.

---

## فناوری‌ها | Tech Stack

- **فریم‌ورک:** Next.js 16 (App Router, Turbopack) + React 19
- **استایل:** SCSS + Bootstrap 5 + Reactstrap (تم RTL)
- **فرم:** Formik + Yup
- **داده:** TanStack React Query + Axios
- **اعلان:** react-toastify
- **آیکون:** react-icons (Remix Icon)

---

## صفحات اصلی | Main Routes

| مسیر | توضیح |
| ------ | ------ |
| `/auth/login` | ورود به حساب کاربری |
| `/auth/forgot-password` | بازیابی رمز عبور |
| `/dashboard` | صفحه‌ی خوش‌آمدگویی داشبورد (محافظت‌شده) |
