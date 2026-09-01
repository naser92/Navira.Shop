import type { HomePromoBanner } from "@/features/home/components/home-promo-banner-grid";

export const homePromoBannersMock: HomePromoBanner[] = [
  { id: "handled-mugs", eyebrow: "برای لحظه‌های گرم", title: "ماگ‌های دسته‌دار", description: "رنگی متناسب با حال‌وهوای تو", badgeText: "پرفروش", image: { src: "/images/hero/ember-terracotta.png", alt: "ماگ سرامیکی دسته‌دار تراکوتا", width: 1024, height: 1536 }, href: "/categories/mugs", ctaLabel: "مشاهده ماگ‌ها", themeKey: "rose", layout: "content-start-image-end", sortOrder: 1, isActive: true },
  { id: "travel-mugs", eyebrow: "همراه همیشگی", title: "تراول ماگ‌های رنگی", description: "نوشیدنی دلخواهت همیشه همراهت", image: { src: "/images/hero/adventure-ocean.png", alt: "تراول ماگ آبی مناسب سفر", width: 1024, height: 1536 }, href: "/categories/travel-mugs", ctaLabel: "خرید تراول ماگ", themeKey: "ocean", layout: "image-start-content-end", sortOrder: 2, isActive: true },
  { id: "double-wall-glasses", eyebrow: "طراحی متفاوت", title: "لیوان‌های دوجداره", description: "زیبایی و حفظ دمای نوشیدنی", badgeText: "جدید", image: { src: "/images/hero/ember-cream.png", alt: "لیوان روشن با طراحی مینیمال", width: 1024, height: 1536 }, href: "/categories/double-wall-glasses", ctaLabel: "مشاهده لیوان‌ها", themeKey: "violet", layout: "content-start-image-end", sortOrder: 3, isActive: true },
  { id: "bottles-and-flasks", eyebrow: "برای ورزش و سفر", title: "فلاسک و قمقمه", description: "سبک، بادوام و همیشه آماده", discountText: "تا ۳۰٪ تخفیف", image: { src: "/images/hero/trail-mint.png", alt: "قمقمه ورزشی سبز نعنایی", width: 1024, height: 1536 }, href: "/categories/bottles-and-flasks", ctaLabel: "مشاهده محصولات", themeKey: "orange", layout: "image-start-content-end", sortOrder: 4, isActive: true },
];

