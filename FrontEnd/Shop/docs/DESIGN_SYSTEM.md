# NaviraShop Design System

## 1. Purpose

این سند قوانین طراحی بصری و رفتاری فرانت‌اند NaviraShop را مشخص می‌کند.  
هدف این Design System ایجاد یک تجربه یکپارچه، قابل نگهداری، ریسپانسیو، RTL-first و سازگار با معماری پروژه است.

این سند باید مرجع اصلی تمام طراحی‌ها، پیاده‌سازی کامپوننت‌ها و توسعه صفحات باشد.

---

## 2. Design Principles

### 2.1. Visual Direction
سبک بصری پروژه باید این ویژگی‌ها را داشته باشد:

- مدرن
- مینیمال
- روشن
- شفاف
- آرام
- محصول‌محور
- مناسب فروشگاه فارسی
- الهام‌گرفته از iOS glassmorphism

### 2.2. Core Experience Rules

- تمرکز اصلی روی تصویر محصول و قیمت باشد.
- جلوه‌های بصری نباید خوانایی را خراب کنند.
- افکت شیشه‌ای باید کنترل‌شده باشد، نه افراطی.
- تجربه کاربری باید سریع، سبک و قابل پیش‌بینی باشد.
- تمام صفحات باید از یک زبان بصری واحد پیروی کنند.
- رابط کاربری باید در موبایل، تبلت و دسکتاپ یکپارچه باشد.
- طراحی باید RTL-first باشد، نه فقط RTL-compatible.

### 2.3. UI Philosophy

- صفحات با layoutهای پایه مشترک ساخته شوند.
- spacing، radius، blur، shadow و border از tokenها بیایند.
- هر کامپوننت باید stateهای کامل داشته باشد: default, hover, active, focus, disabled, loading, error.
- از ساخت UI موردی و بدون الگو جلوگیری شود.
- هیچ بخش از UI نباید ظاهر مستقل و ناسازگار با سیستم داشته باشد.

---

## 3. Brand Tone

NaviraShop یک فروشگاه تخصصی ماگ است.  
رابط کاربری باید حس این موارد را منتقل کند:

- ظرافت
- کیفیت
- تمیزی
- سبک زندگی مدرن
- حس هدیه‌پذیری
- سادگی لوکس
- آرامش بصری

رابط کاربری نباید حس سنگینی، شلوغی، یا فضای تبلیغاتی اغراق‌آمیز داشته باشد.

---

## 4. Direction and Localization

### 4.1. Language
- زبان اصلی: فارسی
- `lang="fa"`
- تمام متون UI فارسی باشند مگر داده‌های سیستمی یا برندهای خارجی

### 4.2. Direction
- کل سیستم باید `dir="rtl"` باشد.
- طراحی باید RTL-first باشد.
- استفاده از `left/right` در CSS فقط در موارد ضروری مجاز است.
- در همه استایل‌ها باید از logical properties استفاده شود:
  - `margin-inline-start`
  - `margin-inline-end`
  - `padding-inline-start`
  - `padding-inline-end`
  - `inset-inline-start`
  - `inset-inline-end`

### 4.3. Content Layout Rules
- تراز پیش‌فرض متن‌ها در بیشتر بخش‌ها راست‌چین باشد.
- برای قیمت، اعداد، شناسه سفارش و داده‌های ساختاریافته می‌توان از تراز مناسب محتوای عددی استفاده کرد.
- در کامپوننت‌های ترکیبی، جهت آیکون و متن باید با RTL هماهنگ باشد.

---

## 5. Responsive Strategy

### 5.1. Responsive Philosophy
- طراحی باید mobile-first باشد.
- تمام کامپوننت‌ها از عرض 320px به بالا قابل استفاده باشند.
- رفتار کامپوننت‌ها باید در breakpointها پایدار و قابل پیش‌بینی باشد.
- هیچ متن، دکمه، badge یا card نباید overflow غیرقابل کنترل داشته باشد.

### 5.2. Breakpoints
```scss
$breakpoint-xs: 360px;
$breakpoint-sm: 576px;
$breakpoint-md: 768px;
$breakpoint-lg: 992px;
$breakpoint-xl: 1200px;
$breakpoint-2xl: 1440px;

### 5.3. Layout Rules
- فاصله‌ها و gridها باید responsive باشند.
- layout نباید در موبایل فقط نسخه فشرده دسکتاپ باشد.
- برای هر صفحه باید نسخه موبایل به صورت مستقل در نظر گرفته شود.
- navigation موبایل و دسکتاپ می‌توانند متفاوت باشند اما باید از یک design language پیروی کنند.

---

## 6. Color System

### 6.1. Core Palette

txt
Background Primary: #F7F8FA
Background Secondary: #EEF2F6
Surface Solid: rgba(255, 255, 255, 0.82)
Surface Glass: rgba(255, 255, 255, 0.56)
Surface Glass Strong: rgba(255, 255, 255, 0.68)

Text Primary: #172033
Text Secondary: #5B6475
Text Muted: #7B8494
Text On Dark: #F8FAFC

Primary: #2F6FED
Primary Hover: #215ED4
Primary Soft: #DCE8FF

Accent Pink: #E78FB3
Accent Mint: #A7E8D8
Accent Peach: #F7C7A8

Border Soft: rgba(255, 255, 255, 0.42)
Border Default: rgba(23, 32, 51, 0.08)
Border Strong: rgba(23, 32, 51, 0.16)

Success: #16A34A
Warning: #F59E0B
Error: #DC2626
Info: #0EA5E9

### 6.2. Color Usage Rules
- رنگ اصلی برای CTAها، لینک‌های مهم و stateهای تعاملی استفاده شود.
- رنگ accent فقط برای تنوع محدود و نقاط تاکید استفاده شود.
- سطح‌های glass باید همیشه همراه border و blur کنترل‌شده باشند.
- متن روی سطح‌های glass باید کنتراست کافی داشته باشد.
- از استفاده بیش از حد از رنگ‌های اشباع و تند خودداری شود.

---

## 7. Typography

### 7.1. Font Family
پیشنهاد اصلی:

txt
Vazirmatn

پیشنهاد جایگزین در صورت نیاز برندینگ متفاوت:

txt
Peyda
IRANSansX
YekanBakh

### 7.2. Typography Rules
- تایپ باید خوانا، متعادل و مناسب فارسی باشد.
- line-height باید برای فارسی کمی بازتر از رابط‌های انگلیسی تنظیم شود.
- از letter-spacing منفی استفاده نشود.
- اندازه فونت‌ها باید token-based باشند.

### 7.3. Type Scale

txt
Display Large: 48px / 56px / 700
Display Medium: 40px / 48px / 700
Heading 1: 32px / 40px / 700
Heading 2: 28px / 36px / 700
Heading 3: 24px / 32px / 600
Heading 4: 20px / 28px / 600
Title Large: 18px / 28px / 600
Title Medium: 16px / 24px / 600
Body Large: 16px / 30px / 400
Body Medium: 15px / 28px / 400
Body Small: 14px / 24px / 400
Caption: 12px / 20px / 400
Label: 13px / 20px / 500
Button: 14px / 20px / 600

### 7.4. Text Behavior
- عنوان‌ها باید کوتاه، واضح و قابل اسکن باشند.
- متن دکمه‌ها نباید طولانی باشد.
- در کارت محصول، عنوان باید حداکثر 2 خط باشد.
- قیمت و وضعیت موجودی باید سریع قابل تشخیص باشند.

---

## 8. Spacing System

### 8.1. Base Scale

txt
4, 8, 12, 16, 20, 24, 32, 40, 48, 56, 64, 80

### 8.2. Usage Rules
- spacing باید فقط از scale سیستم استفاده کند.
- margin و paddingهای دلخواه خارج از scale مجاز نیستند مگر با دلیل مشخص.
- فاصله عمودی بخش‌ها باید بیشتر از فاصله عناصر داخلی باشد.
- موبایل باید spacingهای فشرده‌تر ولی همچنان خوانا داشته باشد.

---

## 9. Radius System

txt
xs: 8px
sm: 12px
md: 16px
lg: 20px
xl: 24px
pill: 999px

### Rules
- بیشتر inputها، buttonها و cardها از `sm` یا `md` استفاده کنند.
- radiusهای خیلی بزرگ فقط برای المان‌های خاص استفاده شوند.
- طراحی نباید حالت overly rounded پیدا کند.

---

## 10. Shadow System

txt
Shadow Soft: 0 8px 24px rgba(15, 23, 42, 0.06)
Shadow Medium: 0 12px 32px rgba(15, 23, 42, 0.10)
Shadow Strong: 0 16px 48px rgba(15, 23, 42, 0.14)

### Rules
- shadow برای تعریف لایه‌ها باشد، نه صرفا تزئین.
- روی glass surfaces از shadow نرم استفاده شود.
- hover elevation باید subtle باشد.

---

## 11. Glassmorphism Rules

### 11.1. Glass Surface Definition
سطوح شیشه‌ای باید شامل این ویژگی‌ها باشند:

- پس‌زمینه نیمه‌شفاف
- blur کنترل‌شده
- border روشن
- shadow نرم
- کنتراست کافی برای متن

### 11.2. Base Glass Token

scss
background: rgba(255, 255, 255, 0.56);
backdrop-filter: blur(24px) saturate(180%);
-webkit-backdrop-filter: blur(24px) saturate(180%);
border: 1px solid rgba(255, 255, 255, 0.42);
box-shadow: 0 12px 40px rgba(15, 23, 42, 0.08);

### 11.3. Usage Rules
Glass effect مجاز است در:

- header
- product card
- cart drawer
- filters panel
- modal
- floating action areas
- mobile bottom navigation

Glass effect نباید باعث این موارد شود:

- افت خوانایی متن
- نویز بیش از حد
- شلوغی بصری
- عدم تمایز بین لایه‌ها

---

## 12. Layout System

### 12.1. Global Layout Building Blocks
تمام صفحات باید با اجزای ساختاری مشترک ساخته شوند:

- `PageContainer`
- `Section`
- `SectionHeader`
- `ContentGrid`
- `Stack`
- `Cluster`
- `AppHeader`
- `AppFooter`

### 12.2. Page Rules
- هیچ صفحه‌ای نباید spacing یا container مستقل و خارج از سیستم بسازد.
- همه صفحات باید از layout primitives مشترک استفاده کنند.
- صفحات باید در موبایل و دسکتاپ از یک منطق ساختاری پیروی کنند.

### 12.3. Width Rules
پیشنهاد برای container اصلی:

txt
Content Max Width: 1280px
Standard Horizontal Padding:
- Mobile: 16px
- Tablet: 24px
- Desktop: 32px

---

## 13. Component Rules

### 13.1. Component Philosophy
- کامپوننت‌ها باید reusable باشند.
- business logic از UI primitives جدا باشد.
- کامپوننت‌های عمومی در `framework/ui` قرار بگیرند.
- کامپوننت‌های feature-specific در `features/*/components` قرار بگیرند.

### 13.2. Required Component States
هر کامپوننت تعاملی باید حداقل این stateها را پوشش دهد:

- default
- hover
- active
- focus-visible
- disabled
- loading
- error (در صورت نیاز)

### 13.3. Common Components
فهرست کامپوننت‌های پایه پیشنهادی:

- Button
- IconButton
- Input
- Textarea
- Select
- Checkbox
- Radio
- Switch
- Tabs
- Badge
- Modal
- Drawer
- Tooltip
- Skeleton
- Spinner
- EmptyState
- Price
- QuantityStepper
- ProductCard
- ProductGallery
- Pagination
- Breadcrumb
- SearchBox

---

## 14. Buttons

### 14.1. Variants
- primary
- secondary
- ghost
- outline
- danger

### 14.2. Rules
- CTA اصلی هر بخش فقط یک button primary داشته باشد.
- buttonها باید ارتفاع ثابت و padding استاندارد داشته باشند.
- متن button نباید باعث تغییر ارتفاع آن شود.
- برای actionهای کوچک‌تر از icon button استفاده شود.

---

## 15. Forms

### 15.1. Rules
- label همیشه واضح و خوانا باشد.
- خطاها در نزدیکی همان فیلد نمایش داده شوند.
- فیلدها باید stateهای focus، error و disabled را واضح نشان دهند.
- inputها در موبایل به اندازه کافی بزرگ باشند.

### 15.2. Validation
- validation باید با پیام فارسی روشن نمایش داده شود.
- پیام خطا کوتاه، مستقیم و بدون ابهام باشد.

---

## 16. Product Card Rules

کارت محصول یکی از مهم‌ترین عناصر سایت است.

### 16.1. Required Content
- تصویر محصول
- عنوان
- قیمت
- وضعیت تخفیف در صورت وجود
- وضعیت موجودی در صورت نیاز
- CTA مناسب

### 16.2. Rules
- نسبت تصویر باید پایدار باشد.
- عنوان حداکثر 2 خط
- قیمت باید برجسته‌تر از متن ثانویه باشد.
- کارت در hover باید subtle feedback داشته باشد.
- CTAها نباید شلوغی ایجاد کنند.

---

## 17. Motion and Interaction

### 17.1. Motion Rules
- انیمیشن‌ها باید کوتاه، نرم و هدفمند باشند.
- motion نباید باعث delay در تعامل شود.
- hover و open/close transitions باید سبک باشند.

### 17.2. Timing
پیشنهاد:

txt
Fast: 120ms
Normal: 180ms
Slow: 240ms

### 17.3. Easing
txt
ease-out
ease-in-out

---

## 18. Accessibility

### 18.1. Minimum Rules
- تمام دکمه‌ها و inputها باید focus-visible مناسب داشته باشند.
- کنتراست متن با پس‌زمینه باید قابل قبول باشد.
- تمام icon-only buttonها باید label قابل دسترس داشته باشند.
- فرم‌ها باید با keyboard قابل استفاده باشند.
- متن روی glass surfaces باید خوانا بماند.

### 18.2. Responsive Accessibility
- tap targetها در موبایل کوچک نباشند.
- فاصله بین عناصر قابل لمس کافی باشد.
- drawerها و modalها باید رفتار keyboard-friendly داشته باشند.

---

## 19. SEO and Content UI Rules

- heading hierarchy باید منطقی باشد.
- صفحات محصول و دسته‌بندی باید ساختار محتوایی روشن داشته باشند.
- متن‌های placeholder نباید نقش label را بگیرند.
- metadata و schema در سطح page رعایت شود.

---

## 20. Implementation Mapping

### 20.1. Design Tokens
این موارد باید در `src/styles/_tokens.scss` تعریف شوند:

- colors
- spacing
- radius
- shadows
- z-index
- typography sizes
- transition durations

### 20.2. Glass Styles
این موارد باید در `src/styles/_glass.scss` تعریف شوند:

- base glass mixin
- strong glass mixin
- dark glass mixin در صورت نیاز
- reusable glass utility classes

### 20.3. Breakpoints and Mixins
این موارد باید در فایل‌های زیر تعریف شوند:

- `src/styles/_breakpoints.scss`
- `src/styles/_mixins.scss`

---

## 21. AI Implementation Rules

هر مدل هوش مصنوعی که روی این پروژه کار می‌کند باید این قواعد را رعایت کند:

1. قبل از ساخت هر کامپوننت، componentهای موجود را بررسی کند.
2. style جدید تکراری نسازد اگر token یا mixin مشابه وجود دارد.
3. تمام خروجی‌ها را RTL-first پیاده‌سازی کند.
4. طراحی را mobile-first انجام دهد.
5. glass effect را فقط از utilityها یا mixinهای مرکزی اعمال کند.
6. از spacing و radius دلخواه خارج از design system استفاده نکند.
7. برای هر component stateهای لازم را در نظر بگیرد.
8. متن‌ها را طوری پیاده کند که در موبایل overflow ایجاد نشود.
9. هر صفحه را با layout primitives مشترک بسازد.
10. قبل از افزودن `use client` دلیل آن را مشخص کند.

---

## 22. Out of Scope

موارد زیر خارج از Design System هستند مگر بعدا تعریف شوند:

- طراحی پنل ادمین
- multi-theme کامل
- dark mode
- design for kiosk/tv
- micro-animations پیچیده برندینگ

---

## 23. Final Rule

اگر هر طراحی، component یا صفحه‌ای با این سند ناسازگار بود، باید قبل از پیاده‌سازی اصلاح یا بازبینی شود.  
این سند مرجع اصلی تصمیمات UI در پروژه NaviraShop است.


برای استفاده درست، کنار این فایل بهتر است `PRD.md` و `ARCHITECTURE.md` هم در همان `docs/` بمانند و در هر پرامپت به هر سه ارجاع بدهید.

مرحله منطقی بعدی این است که `AI_RULES.md` را هم بسازیم، چون همان فایل است که رفتار مدل را کنترل می‌کند تا هر بار UI را با سبک متفاوت نسازد.