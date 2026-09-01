# Codex Worklog

## 2026-09-01 — تکمیل Header و Footer فروشگاه

### تحلیل وضعیت موجود

- پروژه از Next.js App Router، TypeScript strict و SCSS Modules استفاده می‌کند.
- Header اولیه فقط شامل برند، جست‌وجو، ورود و سبد خرید بود؛ Footer وجود نداشت.
- Design tokenها، glass mixinها و PageContainer موجود بودند و مجدداً استفاده شدند.

### تصمیم‌های معماری و فرضیات

- مدل‌های نمایشی layout در `framework/ui/layout/types.ts` مستقل از DTO بک‌اند تعریف شدند.
- داده‌های نمایشی در `src/mocks` قرار گرفتند و از JSX جدا شدند.
- Header به دلیل state منوی موبایل، قفل scroll، Escape و focus اولیه Client Component است؛ Footer به‌صورت Server Component باقی ماند.
- تا زمان آماده‌شدن API و authentication واقعی، اطلاعات کاربر و سبد خرید mock هستند و هیچ منطق احراز هویت جعلی اضافه نشده است.

### فایل‌های ایجادشده

- `src/framework/ui/layout/types.ts`
- `src/mocks/storefront-layout.mock.ts`
- `src/framework/ui/layout/footer/Footer.tsx`
- `src/framework/ui/layout/footer/Footer.module.scss`
- `src/app/page.module.scss`
- `docs/CODEX_WORKLOG.md`

### فایل‌های تغییرکرده

- `src/framework/ui/layout/header/Header.tsx`
- `src/framework/ui/layout/header/Header.module.scss`
- `src/framework/ui/layout/index.ts`
- `src/app/layout.tsx`
- `src/app/page.tsx`
- `docs/CHANGELOG.md`

### قرارداد داده پیشنهادی Backend

#### `GET /api/v1/storefront/layout`

- Method: `GET`
- Request: ندارد
- Response: `ApiResponse<{ header: HeaderDto; footer: FooterDto }>`
- فیلدهای ضروری Header: `navigation[]`, `navigation[].label`, `navigation[].href`, `cartItemCount`
- فیلدهای nullable: `userDisplayName`, `navigation[].children`
- فیلدهای ضروری Footer: `description`, `linkGroups[]`, `contacts[]`, `socialLinks[]`, `trustBadges[]`
- ترتیب آرایه‌ها همان ترتیب نمایش در UI است؛ منو و زیرمنو به‌صورت درختی بازگردانده شوند.
- Empty state: آرایه خالی مجاز است و UI بخش مربوطه را بدون شکست layout حذف می‌کند.
- Error state: layout پایه از cache آخرین پاسخ یا داده fallback محلی استفاده کند؛ خطای فنی مستقیم نمایش داده نشود.
- Cache: پاسخ عمومی با version یا ETag و cache کوتاه‌مدت قابل ارائه است؛ شمارنده سبد خرید باید endpoint/session مستقل داشته باشد.

نمونه خلاصه پاسخ:

```json
{
  "data": {
    "header": { "navigation": [], "cartItemCount": 0, "userDisplayName": null },
    "footer": { "description": "...", "linkGroups": [], "contacts": [], "socialLinks": [], "trustBadges": [] }
  },
  "succeeded": true,
  "statusCode": 200
}
```

### کارهای انجام‌شده

- Header دو‌سطحی desktop و Drawer موبایل RTL-first تکمیل شد.
- scroll صفحه هنگام باز بودن Drawer قفل و بستن با Escape یا backdrop پشتیبانی شد.
- badge سبد، علاقه‌مندی، حالت active، submenu و focus-visible اضافه شد.
- Footer داده‌محور، newsletter UI، گروه لینک‌ها، تماس، شبکه اجتماعی، trust badges و legal links اضافه شد.

### تست‌ها و بررسی‌ها

- `npm run typecheck`: موفق
- `npm run build`: موفق
- تست خودکار component در پروژه تعریف نشده است.

### بدهی فنی و مرحله بعد

- focus trap کامل Drawer پس از اضافه‌شدن primitive مشترک Dialog/Drawer پیاده‌سازی شود.
- active route پس از تکمیل routeها از pathname واقعی استخراج شود.
- فرم newsletter و جست‌وجو به endpointهای واقعی متصل شوند.
- داده mock با mapper و service قرارداد فوق جایگزین شود.
