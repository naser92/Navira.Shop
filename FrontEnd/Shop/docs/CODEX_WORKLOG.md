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

### سازگاری با Sites

- build اولیه Next.js در محیط میزبانی موفق بود اما runtime فایل `dist/server/index.js` را نیاز داشت.
- برای تولید artifact سازگار، `vinext` و plugin رسمی RSC اضافه و React/ReactDOM به نسخه 19 سازگار با Next.js 15 ارتقا یافتند.
- script ساخت، handler تولیدشده توسط vinext را در Worker object دارای متد `fetch` قرار می‌دهد که قرارداد runtime میزبانی است.
- خروجی‌های generated مربوط به Next و vinext از typecheck مستقل پروژه و Git خارج نگه داشته شدند تا typeهای تولیدی دو bundler با هم تداخل نداشته باشند.

## 2026-09-01 — Hero Product Slider چندلایه

### تحلیل و تصمیم معماری

- صفحه خانه Server Component باقی ماند و فقط اسلایدر تعاملی با مرز `use client` پیاده‌سازی شد.
- مدل view، داده mock و `HeroSliderDataSource` از JSX مستقل‌اند؛ بنابراین جایگزینی mock با adapter مربوط به ASP.NET بدون تغییر خود کامپوننت انجام می‌شود.
- برای جلوگیری از افزایش bundle و وابستگی غیرضروری، موتور اسلایدر با React و CSS Modules پیاده‌سازی شد و کتابخانه slider جدیدی اضافه نشد.
- هر اسلاید state رنگ مستقل دارد؛ تغییر رنگ تصویر، قیمت، badge، لینک و تم را یک‌جا تغییر می‌دهد.

### قابلیت‌های تحویل‌شده

- سه اسلاید محصول و مجموعاً نه variant رنگی با تصاویر PNG شفاف اختصاصی.
- autoplay حلقوی، توقف روی hover/focus/interaction و تب غیرفعال، و شروع دوباره پس از تعامل.
- کنترل قبلی/بعدی، pagination، کلیدهای جهت‌دار و Home/End، swipe و ناحیه `aria-live`.
- palette رادیویی معنایی با focus-visible و حالت disabled برای variant ناموجود.
- parallax سبک تصویر و glow و scatter حروف مدل با Pointer Events و `requestAnimationFrame`، بدون setState در حرکت pointer.
- پشتیبانی RTL، mobile-first، `prefers-reduced-motion`، preload تصاویر variant و fallback خطای تصویر.

### قرارداد پیشنهادی Backend

#### `GET /api/v1/storefront/hero-slides`

- Response: `ApiResponse<HeroSlideDto[]>`
- فیلدهای ضروری اسلاید: `id`, `eyebrow`, `title`, `model`, `sortOrder`, `isActive`, `themeKey`, `variants[]`.
- فیلد nullable: `description`.
- فیلدهای ضروری variant: `id`, `name`, `colorHex`, `price.amount`, `price.currency`, `image.url`, `image.alt`, `productUrl`, `isAvailable`؛ فیلد `badge` nullable است.
- backend فقط `themeKey` تأییدشده برمی‌گرداند و frontend آن را به tokenهای امن theme map می‌کند؛ رنگ‌های خام background از API وارد CSS نمی‌شوند.
- ترتیب با `sortOrder` مشخص می‌شود؛ اسلاید غیرفعال، بدون variant یا variant ناموجود به‌ترتیب حذف/غیرفعال می‌شوند.
- Empty state: پاسخ خالی UI جایگزین کوتاه نمایش می‌دهد. Error state: استفاده از cache آخرین پاسخ و سپس داده fallback محلی؛ خطای فنی مستقیم به کاربر نمایش داده نمی‌شود.
- Cache پیشنهادی: cache عمومی کوتاه‌مدت با `ETag` و `stale-while-revalidate`؛ موجودی و قیمت در صورت حساسیت تجاری باید TTL کوتاه‌تری داشته باشند.

نمونه خلاصه پاسخ:

```json
{
  "data": [{
    "id": "adventure-tumbler",
    "title": "ماگ سفری Adventure",
    "model": "ADVENTURE",
    "themeKey": "rose",
    "sortOrder": 10,
    "isActive": true,
    "variants": [{
      "id": "rose",
      "name": "رز",
      "colorHex": "#c9697e",
      "price": { "amount": 1890000, "currency": "IRR" },
      "image": { "url": "/media/adventure-rose.png", "alt": "ماگ سفری Adventure به رنگ رز" },
      "productUrl": "/products/adventure-rose",
      "badge": "پرفروش",
      "isAvailable": true
    }]
  }],
  "succeeded": true,
  "statusCode": 200
}
```

### فایل‌های اصلی

- `src/features/home/components/hero-product-slider/*`
- `src/features/home/data/hero-slider.data-source.ts`
- `src/mocks/home-hero-slides.mock.ts`
- `public/images/hero/*`
- `src/app/page.tsx` و `src/app/page.module.scss`

### دارایی‌های تولیدشده

- تصاویر با قابلیت داخلی ImageGen و با سبک یکپارچه studio product photography، نمای سه‌ربع، نور نرم، سایه طبیعی و پس‌زمینه شفاف تولید شدند.
- سه خانواده prompt شامل tall travel tumbler، compact ceramic mug و athletic water bottle بود؛ برای هر خانواده سه رنگ مستقل ساخته و مستقیماً به‌عنوان asset پروژه استفاده شد.

### آزمون و بدهی فنی

- `npm run typecheck`: موفق.
- `npm run build:next`: موفق و route خانه به‌صورت static prerender شد.
- پس از آماده‌شدن API، mock data source با adapter واقعی، mapper قیمت و allow-list مربوط به `themeKey` جایگزین شود.

## 2026-09-01 — نوار دسته‌بندی‌های سریع Home

### معماری و رفتار

- `HomeCategoryStrip` بلافاصله پس از Hero و مستقل از state و منبع داده اسلایدر قرار گرفت.
- کامپوننت Server Component است؛ داده را فقط از props می‌گیرد و mock از طریق `HomeCategoryDataSource` در صفحه Server دریافت می‌شود.
- ده دسته‌بندی فعال با `sortOrder` مرتب می‌شوند؛ empty state کاملاً `null` است و فضای اضافه ایجاد نمی‌کند.
- icon registry فقط کلیدهای کنترل‌شده lucide را می‌پذیرد و fallback امن دارد. theme registry نیز `themeKey` را به palette محدود SCSS نگاشت می‌کند.
- موبایل و تبلت از یک ردیف scrollable با touch، scroll snap، آیتم نیمه‌قابل‌مشاهده و scrollbar مخفی استفاده می‌کنند؛ در نمایشگر عریض آیتم‌ها در یک ردیف متعادل پخش می‌شوند.
- هر آیتم یک Link کامل و شامل دایره، آیکن تزئینی، متن واقعی، badge اختیاری، focus ring و Featured marker غیررنگی است.

### قرارداد Backend پیشنهادی

#### `GET /api/v1/storefront/home/categories`

- حداکثر ۱۵ آیتم Featured برای Home بازگردانده شود؛ ترتیب قطعی براساس `sortOrder` صعودی است.
- دسته‌بندی اصلی با موجودیت category مشخص می‌شود و `isFeatured` فقط واجد شرایط بودن برای Home را نشان می‌دهد؛ آیتم غیرفعال یا Featured=false در پاسخ Home حذف شود.
- کلیدهای مجاز `iconKey`: `laptop`, `shirt`, `cooking-pot`, `dumbbell`, `pen`, `sparkles`, `baby`, `gamepad`, `book`, `gift`, `grid`.
- `themeKey` فقط یکی از `blue`, `purple`, `orange`, `green`, `teal`, `pink`, `yellow`, `indigo` باشد.
- `iconType=icon` به `iconKey` معتبر نیاز دارد. برای `iconType=image` فقط URL رسانه معتبر، alt و ابعاد ارسال شود؛ SVG/HTML/JS اجرایی پذیرفته نمی‌شود. کلید یا تصویر نامعتبر در frontend با آیکن fallback جایگزین می‌شود.
- `url` به‌صورت پیش‌فرض مسیر داخلی allow-listed است؛ URL خارجی فقط با پرچم و allow-list دامنه در نسخه آینده پذیرفته شود.
- cache پیشنهادی: `public, max-age=300, stale-while-revalidate=3600` همراه `ETag`. فیلد `version` برای invalidation و سازگاری mapper استفاده شود.
- پاسخ خالی بخش را مخفی می‌کند؛ خطا از fallback/cache لایه data source استفاده می‌کند و خطای خام در Home نمایش داده نمی‌شود.

```json
{
  "version": "2026-09-01",
  "items": [{
    "id": "digital",
    "slug": "digital-products",
    "title": "کالای دیجیتال",
    "url": "/categories/digital-products",
    "iconType": "icon",
    "iconKey": "laptop",
    "iconUrl": null,
    "iconAlt": null,
    "themeKey": "blue",
    "badgeText": null,
    "sortOrder": 1,
    "isFeatured": true
  }]
}
```

### فایل‌ها و آزمون‌ها

- کامپوننت: `src/features/home/components/home-category-strip/`
- mock: `src/mocks/home-categories.mock.ts`
- data source: `src/features/home/data/home-category.data-source.ts`
- `npm run typecheck`: موفق.
- `npm run lint`: موفق، بدون warning یا error کد؛ فقط پیام deprecation مربوط به command خود Next.js 15 نمایش داده شد.
- `npm run build:next`: موفق؛ صفحه Home همچنان static prerender است.
- سناریوهای ساختاری شامل ۱، ۳، ۱۰، بیش از ۱۵ و empty با همان props/filter/sort پوشش‌پذیرند. بررسی مرورگری خودکار چند viewport در repository فعلی runner ندارد و باید در تست E2E آینده اضافه شود.

## 2026-09-01 — چهار بنر تبلیغاتی Home

### معماری و UI

- `HomePromoBannerGrid` بعد از دسته‌بندی‌ها و مستقل از Hero و Category Strip قرار گرفت.
- Grid، Card و data source سروری هستند؛ فقط `PromoBannerImage` برای مدیریت `onError` و fallback واقعی یک Client Component کوچک است.
- چهار layout و پنج theme از registry کنترل‌شده انتخاب می‌شوند و Backend امکان ارسال CSS، HTML یا JSX ندارد.
- داده mock چهار کمپین ماگ، تراول ماگ، لیوان و قمقمه را پوشش می‌دهد و از تصاویر PNG شفاف تولیدشده و موجود پروژه استفاده می‌کند؛ متن تبلیغاتی کاملاً HTML است.
- موبایل یک ردیف swipeable با scroll snap، تبلت grid دو در دو و Desktop عریض چهار ستون برابر دارد. Empty state بخش را کاملاً حذف می‌کند.
- کل کارت Link است؛ focus ring، لینک خارجی امن، alt تصویر، fallback، reduced motion و hover بدون layout shift پشتیبانی می‌شوند.

### قرارداد Backend پیشنهادی

#### `GET /api/v1/storefront/home/promo-banners`

- حداکثر چهار بنر فعال و معتبر، مرتب‌شده با `sortOrder` صعودی، برای Home بازگردانده شود.
- زمان شروع و پایان کمپین در Backend ارزیابی شود و بنر خارج از بازه در `items` قرار نگیرد.
- `themeKey`: یکی از `rose`, `ocean`, `violet`, `orange`, `sage`.
- `layoutKey`: یکی از `content-start-image-end`, `image-start-content-end`, `content-top-image-bottom`, `centered-overlay`.
- فیلدهای ضروری: `id`, `title`, `imageUrl`, `imageAlt`, `url`, `ctaLabel`, `themeKey`, `layoutKey`, `sortOrder`؛ توضیح، eyebrow، badge، discount، mobile image و logo nullable هستند.
- URL داخلی باید مسیر allow-listed باشد. لینک خارجی فقط با HTTPS، دامنه مجاز و `openInNewTab=true` پذیرفته شود.
- تصویر باید media URL معتبر با alt و ابعاد باشد؛ تصویر خراب در Frontend fallback غیرشکننده نمایش می‌دهد. متن یا watermark داخل asset استفاده نشود.
- cache پیشنهادی: `public, max-age=300, stale-while-revalidate=3600` همراه `ETag` و version برای invalidation.
- Empty state بخش را پنهان می‌کند؛ در خطا data source از cache آخرین پاسخ یا fallback محلی استفاده کرده و خطای خام نمایش داده نمی‌شود.

```json
{
  "version": "2026-09-01",
  "items": [{
    "id": "handled-mugs",
    "eyebrow": "برای لحظه‌های گرم",
    "title": "ماگ‌های دسته‌دار",
    "description": "رنگی متناسب با حال‌وهوای تو",
    "badgeText": "پرفروش",
    "discountText": null,
    "imageUrl": "/media/banners/handled-mugs.webp",
    "mobileImageUrl": null,
    "imageAlt": "مجموعه‌ای از ماگ‌های دسته‌دار رنگی",
    "url": "/categories/mugs",
    "ctaLabel": "مشاهده ماگ‌ها",
    "themeKey": "rose",
    "layoutKey": "content-start-image-end",
    "sortOrder": 1,
    "openInNewTab": false
  }]
}
```

### فایل‌ها و Verification

- کامپوننت: `src/features/home/components/home-promo-banner-grid/`
- mock: `src/mocks/home-promo-banners.mock.ts`
- data source: `src/features/home/data/home-promo-banner.data-source.ts`
- افزودن بنر با افزودن یک object معتبر به منبع داده انجام می‌شود؛ تصویر نهایی با تغییر `image.src`, `alt`, `width`, `height` جایگزین می‌شود.
- `npm run typecheck`: موفق.
- `npm run lint`: موفق و بدون warning/error کد؛ پیام deprecation خود `next lint` باقی است.
- `npm run build:next`: موفق؛ Home به‌صورت static prerender شد.
- تست visual خودکار viewport در پروژه runner ندارد؛ افزودن Playwright و screenshot regression بدهی مرحله بعد است.
