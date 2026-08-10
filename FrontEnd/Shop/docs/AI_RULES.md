# AI Rules - NaviraShop

## 1. Purpose

این سند قوانین اجرایی استفاده از هوش مصنوعی در پروژه NaviraShop را مشخص می‌کند.

هدف این قوانین:

- حفظ معماری پروژه
- جلوگیری از تولید کد تکراری
- جلوگیری از تغییر سلیقه‌ای Design System
- حفظ قرارداد API بین Next.js و ASP.NET 9
- کاهش خطاهای امنیتی و معماری
- تولید کد قابل نگهداری، تست‌پذیر و type-safe

این سند باید همراه با فایل‌های زیر به عنوان Context در اختیار هوش مصنوعی قرار گیرد:

- `docs/PRD.md`
- `docs/ARCHITECTURE.md`
- `docs/DESIGN_SYSTEM.md`
- `docs/API_CONTRACT.md`

---

## 2. General Rules

1. قبل از هرگونه کدنویسی، فایل‌ها و ساختار مرتبط با تسک را بررسی کن.
2. قبل از ساخت کامپوننت، سرویس، hook یا type جدید، وجود نمونه مشابه را جست‌وجو کن.
3. بدون دلیل فنی، معماری، نام‌گذاری یا ساختار پوشه‌ها را تغییر نده.
4. تغییرات را محدود به محدوده تسک نگه دار.
5. از refactor unrelated خودداری کن.
6. هیچ فایل یا تغییری را بدون درخواست صریح کاربر حذف نکن.
7. کد تولیدشده باید با نسخه‌های فعلی پروژه سازگار باشد.
8. از اضافه‌کردن dependency جدید بدون دلیل و اعلام صریح استفاده نکن.
9. در صورت وجود ابهام مؤثر بر معماری یا رفتار محصول، قبل از پیاده‌سازی سؤال بپرس.
10. فرضیات مهم را در پاسخ نهایی اعلام کن.

---

## 3. Required Context Review

قبل از پیاده‌سازی هر تسک، این موارد را بررسی کن:
```text
1. فایل‌های مرتبط با feature
2. کامپوننت‌ها و hookهای مشابه
3. typeها و DTOهای موجود
4. tokenها و mixinهای Sass
5. API service و endpointهای مشابه
6. قوانین PRD
7. قوانین ARCHITECTURE
8. قوانین DESIGN_SYSTEM
9. قوانین API_CONTRACT

اگر اطلاعات کافی در مستندات وجود ندارد:

- حدس معماری نزن.
- راه‌حل موقت را به عنوان تصمیم قطعی ثبت نکن.
- ابهام را واضح گزارش کن.
- در صورت امکان، کم‌ریسک‌ترین رفتار سازگار با کد موجود را انتخاب کن.

---

## 4. Architecture Rules

پروژه باید از Feature-based Modular Architecture استفاده کند.

ساختار پیشنهادی:

text
src/
├── app/
├── core/
├── framework/
├── features/
└── styles/

### 4.1. `app`

مناسب برای:

- routeها
- layoutها
- pageها
- loading state
- error boundary
- metadata
- route-level composition

در `app` نباید منطق پیچیده بیزینسی، دسترسی مستقیم به دیتابیس یا تعریف componentهای feature-specific قرار گیرد.

### 4.2. `core`

مناسب برای زیرساخت‌های عمومی و مستقل از feature:

- HTTP client
- تنظیمات runtime
- authentication infrastructure
- error handling
- shared types
- query client
- logging
- utilityهای عمومی
- configuration

کد `core` نباید به یک feature خاص وابسته باشد.

### 4.3. `framework`

مناسب برای abstractionها و componentهای عمومی رابط کاربری:

- Button
- Input
- Modal
- Drawer
- Table
- Skeleton
- layout primitives
- shared hooks
- shared UI utilities

کد `framework` نباید منطق اختصاصی محصول یا سفارش خاصی را بشناسد.

### 4.4. `features`

هر feature باید مالک منطق بیزینسی خودش باشد.

ساختار نمونه:

text
features/
└── product/
├── api/
├── components/
├── hooks/
├── store/
├── types/
├── utils/
└── index.ts

قواعد:

- featureها نباید به implementation داخلی featureهای دیگر وابسته شوند.
- استفاده از public API یک feature مجاز است.
- import مستقیم از فایل‌های داخلی feature دیگر مجاز نیست.
- منطق مشترک باید به `core` یا `framework` منتقل شود، نه به یک feature تصادفی.

---

## 5. Dependency Direction

جهت وابستگی باید تا حد امکان به شکل زیر باشد:

text
app -> features -> core
app -> framework
features -> framework
framework -> core
core -> external libraries

قواعد:

- `core` نباید به `features` وابسته باشد.
- `framework` نباید به `features` وابسته باشد.
- componentهای عمومی نباید DTO یا use case اختصاصی یک feature را import کنند.
- circular dependency مجاز نیست.
- import aliasهای پروژه را به importهای نسبی طولانی ترجیح بده.

---

## 6. Next.js Rules

### 6.1. Server Components

به صورت پیش‌فرض از Server Component استفاده کن.

Server Component برای موارد زیر اولویت دارد:

- SEO
- دریافت داده اولیه
- metadata
- محتوای قابل crawl
- صفحه‌های محصول و دسته‌بندی
- layoutهای بدون تعامل محلی

### 6.2. Client Components

فقط زمانی از `"use client"` استفاده کن که یکی از موارد زیر لازم باشد:

- `useState`
- `useEffect`
- event handler
- browser API
- TanStack Query hook
- Redux hook
- تعاملات drag, modal, drawer یا form
- animation وابسته به state

قواعد:

1. `"use client"` را در بالاترین سطح ممکن استفاده نکن.
2. بخش تعاملی را به کوچک‌ترین component ممکن محدود کن.
3. داده و منطق غیرتعاملی را در Server Component نگه دار.
4. از تبدیل کل page به Client Component بدون دلیل خودداری کن.

### 6.3. Data Fetching

برای داده‌های Server-rendered:

- دریافت داده را در Server Component یا لایه service انجام بده.
- برای تعاملات کلاینتی از TanStack Query استفاده کن.
- در صورت استفاده هم‌زمان از SSR و TanStack Query، hydration را مطابق الگوی موجود پروژه انجام بده.
- یک endpoint را هم‌زمان با چند روش متفاوت پیاده‌سازی نکن مگر دلیل مشخص وجود داشته باشد.

---

## 7. API Rules

تمام API callها باید از HTTP client مرکزی عبور کنند:

text
core/api/http-client.ts

قواعد:

1. استفاده مستقیم از `fetch` یا `axios.create` در feature مجاز نیست، مگر در زیرساخت مرکزی.
2. URLها باید در فایل endpoint همان feature تعریف شوند.
3. API service باید در مسیر `features/{feature}/api/` قرار گیرد.
4. component نباید مستقیماً API call انجام دهد.
5. type تمام requestها و responseها باید مشخص باشد.
6. از `any` برای API response یا request استفاده نکن.
7. خروجی API باید از `ApiResponse<T>` یا مدل استاندارد قرارداد پروژه استفاده کند.
8. خطاهای API نباید در هر component به صورت تکراری مدیریت شوند.
9. وضعیت‌های loading، empty، error و success باید برای workflowهای داده‌ای در نظر گرفته شوند.
10. پیام خطا نباید بدون تبدیل مناسب مستقیماً به کاربر نمایش داده شود.

نمونه ساختار:

text
features/product/
├── api/
│   ├── product.endpoints.ts
│   ├── product.service.ts
│   └── product.queries.ts
└── types/
└── product.dto.ts

---

## 8. Authentication and Authorization

احراز هویت پروژه با Keycloak انجام می‌شود.

قواعد:

1. توکن را در `localStorage` ذخیره نکن مگر تصمیم رسمی پروژه چنین چیزی را الزام کند.
2. توکن و session باید از الگوی امن مصوب پروژه پیروی کنند.
3. منطق refresh token نباید در componentها تکرار شود.
4. `401 Unauthorized` باید در لایه مرکزی authentication/API مدیریت شود.
5. `403 Forbidden` باید از خطای authentication جدا باشد.
6. مخفی‌کردن یک button جایگزین authorization سمت سرور نیست.
7. هر permission حساس باید در backend نیز enforce شود.
8. از نمایش اطلاعات خصوصی قبل از تکمیل بررسی دسترسی خودداری کن.
9. claimها را بدون type و validation مصرف نکن.
10. نام permissionها باید با قرارداد backend یکسان باشد.

---

## 9. TypeScript Rules

1. حالت strict پروژه را رعایت کن.
2. برای داده‌های ورودی و خروجی type صریح تعریف کن.
3. از `any` استفاده نکن.
4. در صورت ناشناخته‌بودن داده، از `unknown` همراه با type guard استفاده کن.
5. typeهای API را از typeهای نمایش UI جدا نگه دار، اگر تفاوت معنادار دارند.
6. تبدیل DTO به View Model باید در service، mapper یا utility مشخص انجام شود.
7. از type assertion بی‌دلیل مانند `as SomeType` استفاده نکن.
8. union type را به objectهای مبهم ترجیح بده.
9. نام typeها باید دامنه و کاربردشان را مشخص کند.
10. برای داده nullable، رفتار `null` و `undefined` را صریح تعیین کن.

---

## 10. State Management Rules

### 10.1. Local State

برای stateهایی که فقط در یک component استفاده می‌شوند، از state محلی استفاده کن.

نمونه:

- باز یا بسته‌بودن modal
- مقدار موقت input
- tab فعال
- وضعیت نمایش فیلترها

### 10.2. TanStack Query

برای server state استفاده شود:

- product data
- category data
- cart synchronization
- order history
- user profile
- pagination
- mutations

### 10.3. Redux Toolkit

Redux فقط برای client state مشترک و پایدار استفاده شود.

نمونه:

- cart state در صورت نیاز محصول
- تنظیمات UI سراسری
- وضعیت navigation
- stateهای cross-feature

داده‌ای که منبع اصلی آن API است، نباید بدون دلیل در Redux duplicate شود.

---

## 11. UI and Design System Rules

تمام UI باید با `docs/DESIGN_SYSTEM.md` هماهنگ باشد.

قواعد:

1. RTL-first پیاده‌سازی کن.
2. از CSS logical properties استفاده کن.
3. از `left` و `right` تا حد امکان استفاده نکن.
4. طراحی mobile-first باشد.
5. از tokenهای مرکزی رنگ، spacing، radius، shadow و typography استفاده کن.
6. مقدارهای تکراری Sass را در componentها hardcode نکن.
7. برای glass effect از mixin یا token مرکزی استفاده کن.
8. component جدید را قبل از بررسی `framework/ui` نساز.
9. تمام componentهای تعاملی باید stateهای لازم را داشته باشند:
   - default
   - hover
   - active
   - focus-visible
   - disabled
   - loading
   - error
10. text نباید در mobile overflow یا overlap ایجاد کند.
11. برای icon-only button از `aria-label` و tooltip مناسب استفاده کن.
12. برای buttonهای ابزارمحور از icon library موجود پروژه استفاده کن.
13. از ساخت card تو در تو و تزئینات غیرضروری خودداری کن.
14. از gradient، blur یا shadow صرفاً برای تزئین و بدون نیاز طراحی استفاده نکن.
15. متن‌های رابط کاربری فارسی و سازگار با لحن برند باشند.

---

## 12. Sass and Styling Rules

1. هر component باید از SCSS Module خودش استفاده کند.
2. style عمومی فقط برای reset، tokenها، typography و utilityهای مصوب استفاده شود.
3. selectorهای global غیرضروری نساز.
4. nesting را محدود و خوانا نگه دار.
5. از `!important` استفاده نکن مگر در یک مورد مستند و ضروری.
6. breakpointها باید از فایل مرکزی خوانده شوند.
7. z-index باید از scale مرکزی استفاده کند.
8. animation duration و easing باید token-based باشند.
9. رنگ، فاصله و radius را در component به صورت عدد خام تکرار نکن.
10. media queryها باید mobile-first باشند.

---

## 13. Component Design Rules

قبل از ساخت component جدید:

1. componentهای موجود را جست‌وجو کن.
2. تفاوت واقعی component جدید با نمونه موجود را مشخص کن.
3. مشخص کن component عمومی است یا feature-specific.
4. stateهای موردنیاز را فهرست کن.
5. contract ورودی و خروجی را type کن.

قواعد component:

- component باید یک مسئولیت روشن داشته باشد.
- props باید حداقل و معنادار باشند.
- منطق بیزینسی در component نمایشی قرار نگیرد.
- component نباید به URL یا API خاصی hardcode شود.
- componentهای عمومی نباید متن محصول خاصی را به صورت ثابت داشته باشند.
- componentهای سنگین باید در صورت نیاز lazy load شوند.
- component باید در loading و empty state رفتار مشخص داشته باشد.

---

## 14. Forms and User Input

1. تمام ورودی‌های کاربر باید validate شوند.
2. validation سمت کلاینت جایگزین validation سمت backend نیست.
3. پیام خطا باید نزدیک فیلد مربوطه نمایش داده شود.
4. خطاها باید برای screen reader قابل دسترسی باشند.
5. فرم نباید با submit دوباره، mutation تکراری ناخواسته ایجاد کند.
6. هنگام submit، loading و disabled state مشخص باشد.
7. مقدارهای حساس در log چاپ نشوند.
8. داده‌های کاربر قبل از نمایش یا ارسال، طبق نیاز sanitize شوند.
9. خطای backend باید به پیام قابل فهم برای کاربر تبدیل شود.
10. فرم‌های چندمرحله‌ای باید state و recovery مشخص داشته باشند.

---

## 15. Error Handling and Logging

قواعد خطا:

- خطاهای قابل پیش‌بینی را کنترل کن.
- خطا را silently swallow نکن.
- خطای فنی را مستقیماً به کاربر نمایش نده.
- برای خطاهای عمومی پیام fallback داشته باش.
- خطاهای authorization را با خطاهای validation اشتباه نگیر.
- loading بی‌نهایت ایجاد نکن.
- برای retry فقط خطاهای مناسب retry شوند.
- اطلاعات حساس در error message، log یا telemetry قرار نگیرد.

در production:

- token
- password
- اطلاعات پرداخت
- اطلاعات شخصی غیرضروری
- response کامل سرویس‌های حساس

نباید در log ثبت شوند.

---

## 16. Testing Rules

برای هر تغییر، سطح تست متناسب با ریسک آن انتخاب شود.

حداقل تست‌های مورد انتظار:

- utilityها و mapperها: unit test
- API serviceها: تست موفقیت و خطا
- componentهای تعاملی: تست stateها و eventها
- فرم‌ها: تست validation و submit
- مسیرهای حساس خرید: تست workflow
- authorization: تست دسترسی مجاز و غیرمجاز

تغییرات زیر نیاز به توجه ویژه دارند:

- authentication
- authorization
- cart
- checkout
- payment
- order creation
- قیمت و تخفیف
- موجودی محصول

---

## 17. Performance Rules

1. از Client Component غیرضروری جلوگیری کن.
2. داده‌های غیرضروری را به browser ارسال نکن.
3. تصویرها را با قابلیت‌های مناسب Next.js بهینه کن.
4. از re-render غیرضروری جلوگیری کن.
5. query keyها را پایدار و معنادار تعریف کن.
6. pagination و infinite query را بر اساس نیاز واقعی انتخاب کن.
7. importهای سنگین را در مسیر اولیه page وارد نکن.
8. از duplicate request جلوگیری کن.
9. برای لیست‌های بزرگ virtualization را در صورت نیاز بررسی کن.
10. عملکرد را با حدس اصلاح نکن؛ ابتدا علت را بررسی کن.

---

## 18. Security Rules

1. secretها را داخل source code قرار نده.
2. متغیرهای محیطی public و private را از هم جدا کن.
3. ورودی کاربر را قابل اعتماد فرض نکن.
4. authorization را فقط به UI visibility محدود نکن.
5. از نمایش stack trace به کاربر جلوگیری کن.
6. داده حساس را در query string قرار نده مگر ضرورت فنی وجود داشته باشد.
7. redirectهای ورودی کاربر را validate کن.
8. محتوای HTML را بدون sanitization مناسب render نکن.
9. CORS و CSRF را مطابق معماری authentication پروژه رعایت کن.
10. هر endpoint حساس باید مسئولیت و سطح دسترسی مشخص داشته باشد.

---

## 19. File and Naming Rules

نام‌گذاری باید با convention فعلی پروژه هماهنگ باشد.

قواعد پیشنهادی:

text
component-name.tsx
component-name.module.scss
feature.service.ts
feature.endpoints.ts
feature.dto.ts
use-feature.ts
feature.slice.ts

قواعد:

- نام فایل‌ها در صورت نبود convention متفاوت، kebab-case باشند.
- نام componentها PascalCase باشند.
- نام functionها و variableها camelCase باشند.
- نام constantهای سراسری UPPER_SNAKE_CASE باشند.
- نام‌های مبهم مانند `data`, `result`, `item` را در scopeهای بزرگ استفاده نکن.
- نام feature باید دامنه بیزینسی را مشخص کند.

---

## 20. Git and Change Scope

1. تغییرات مرتبط را در یک مجموعه مشخص نگه دار.
2. فایل‌های unrelated را تغییر نده.
3. dependency یا config جدید را بدون دلیل اضافه نکن.
4. تغییرات generated را بررسی کن.
5. قبل از اعلام اتمام، diff را بررسی کن.
6. فایل‌های ناخواسته و debug code را حذف کن.
7. تغییرات کاربر را بازنویسی یا حذف نکن.
8. در صورت مشاهده تغییرات قبلی در فایل موردنظر، آن‌ها را حفظ و با دقت ادغام کن.

---

## 21. Required Verification

قبل از گزارش اتمام، در صورت وجود scriptهای پروژه این موارد را اجرا کن:

bash
npm run lint
npm run typecheck
npm run test
npm run build

اگر هر script وجود ندارد:

- آن را اجرا نکن.
- نبود آن را در گزارش نهایی اعلام کن.

در صورت خطا:

1. علت خطا را بررسی کن.
2. خطاهای ایجادشده توسط تغییر خودت را اصلاح کن.
3. خطاهای unrelated را از خطاهای مربوط به تسک جدا گزارش کن.
4. هرگز صرفاً برای سبزشدن build، type safety یا رفتار صحیح را حذف نکن.

---

## 22. Required Final Response

پس از پایان هر تسک، پاسخ نهایی باید شامل این موارد باشد:

1. خلاصه تغییرات انجام‌شده
2. فایل‌های اصلی تغییرکرده
3. تصمیم‌های معماری مهم
4. تست‌ها و commandهای اجراشده
5. نتیجه تست‌ها
6. فرضیات یا محدودیت‌های باقی‌مانده

قالب پیشنهادی:

text
تغییرات:
- ...

فایل‌های اصلی:
- ...

بررسی انجام‌شده:
- ...

تست‌ها:
- `npm run ...`: موفق / ناموفق / اجرا نشد

محدودیت یا فرض:
- ...

---

## 23. Prohibited Behaviors

هوش مصنوعی نباید:

- بدون بررسی repository کدنویسی کند.
- component تکراری بسازد.
- API call را داخل UI پراکنده کند.
- از `any` برای پنهان‌کردن خطای type استفاده کند.
- بدون دلیل `"use client"` اضافه کند.
- معماری پروژه را به معماری دلخواه خود تغییر دهد.
- tokenهای Design System را دور بزند.
- secret یا token را در کد قرار دهد.
- تغییرات unrelated ایجاد کند.
- فایل‌های موجود را بدون بررسی بازنویسی کند.
- خطاهای build یا lint را نادیده بگیرد.
- خطای امنیتی را با راه‌حل صرفاً ظاهری در frontend پنهان کند.
- نتیجه تست اجرا‌نشده را موفق اعلام کند.

---

## 24. Decision Priority

در صورت تعارض بین دستورها، اولویت تصمیم‌گیری به ترتیب زیر است:

1. نیازمندی صریح و جدید کاربر
2. امنیت و الزامات backend
3. `docs/ARCHITECTURE.md`
4. `docs/API_CONTRACT.md`
5. `docs/DESIGN_SYSTEM.md`
6. `docs/PRD.md`
7. conventionهای موجود repository
8. پیشنهاد عمومی هوش مصنوعی

در صورت تعارض واقعی بین مستندات، قبل از تغییر معماری موضوع را گزارش کن و تصمیم نهایی را از کاربر بگیر.

---

## 25. Final Principle

هوش مصنوعی در این پروژه باید مانند عضوی از تیم فنی عمل کند:

- ابتدا context را بخواند.
- سپس کد موجود را بررسی کند.
- بعد کم‌ریسک‌ترین راه‌حل سازگار را انتخاب کند.
- تغییرات محدود و قابل توضیح ایجاد کند.
- نتیجه را با تست و گزارش شفاف تحویل دهد.

هیچ کدی صرفاً به دلیل کوتاه‌بودن یا سریع‌بودن، بر معماری، امنیت، خوانایی و قابلیت نگهداری اولویت ندارد.
