import type { HeroSlide } from "@/features/home/components/hero-product-slider";

export const homeHeroSlidesMock: HeroSlide[] = [
  {
    id: "adventure-tumbler", eyebrow: "برای لحظه‌های بیرون از خانه", title: "ماگ سفری Adventure", model: "ADVENTURE", sortOrder: 10, isActive: true,
    description: "سبک، خوش‌دست و همراه همیشگی نوشیدنی‌های گرم و سرد؛ از مسیر روزانه تا سفرهای دور.",
    theme: { background: "#f7e8ec", surface: "#fff9f7", accent: "#b74768", ink: "#392331", glow: "#efb6c6" },
    variants: [
      { id: "rose", name: "رز", color: "#c9697e", price: "۱٬۸۹۰٬۰۰۰ تومان", badge: "پرفروش", href: "/products/adventure-rose", isAvailable: true, image: { src: "/images/hero/adventure-rose.png", alt: "ماگ سفری Adventure به رنگ رز", width: 1024, height: 1536 } },
      { id: "ocean", name: "اقیانوسی", color: "#2f86a6", price: "۱٬۹۲۰٬۰۰۰ تومان", href: "/products/adventure-ocean", isAvailable: true, image: { src: "/images/hero/adventure-ocean.png", alt: "ماگ سفری Adventure آبی اقیانوسی", width: 1024, height: 1536 } },
      { id: "sage", name: "سیج", color: "#86a48d", price: "۱٬۸۹۰٬۰۰۰ تومان", href: "/products/adventure-sage", isAvailable: true, image: { src: "/images/hero/adventure-sage.png", alt: "ماگ سفری Adventure سبز سیج", width: 1024, height: 1536 } },
    ],
  },
  {
    id: "ember-mug", eyebrow: "آرامش در هر جرعه", title: "ماگ سرامیکی Ember", model: "EMBER", sortOrder: 20, isActive: true,
    description: "فرم جمع‌وجور، لعاب مات و حس گرم سرامیک؛ برای قهوه‌ای که قرار نیست با عجله نوشیده شود.",
    theme: { background: "#f2e6d5", surface: "#fffaf1", accent: "#b55736", ink: "#3d2b24", glow: "#efc299" },
    variants: [
      { id: "cream", name: "کرم", color: "#e8d8bd", price: "۱٬۲۹۰٬۰۰۰ تومان", badge: "جدید", href: "/products/ember-cream", isAvailable: true, image: { src: "/images/hero/ember-cream.png", alt: "ماگ سرامیکی Ember کرم", width: 1024, height: 1536 } },
      { id: "terracotta", name: "تراکوتا", color: "#b95839", price: "۱٬۳۹۰٬۰۰۰ تومان", href: "/products/ember-terracotta", isAvailable: true, image: { src: "/images/hero/ember-terracotta.png", alt: "ماگ سرامیکی Ember تراکوتا", width: 1024, height: 1536 } },
      { id: "navy", name: "سرمه‌ای", color: "#213f57", price: "۱٬۳۹۰٬۰۰۰ تومان", href: "/products/ember-navy", isAvailable: true, image: { src: "/images/hero/ember-navy.png", alt: "ماگ سرامیکی Ember سرمه‌ای", width: 1024, height: 1536 } },
    ],
  },
  {
    id: "trail-bottle", eyebrow: "طراوت در حرکت", title: "بطری ورزشی Trail", model: "TRAIL", sortOrder: 30, isActive: true,
    theme: { background: "#e6f2ef", surface: "#f8fffb", accent: "#2d8875", ink: "#1e3835", glow: "#a9ded1" },
    variants: [
      { id: "lavender", name: "یاسی", color: "#a18ac0", price: "۱٬۵۹۰٬۰۰۰ تومان", href: "/products/trail-lavender", isAvailable: true, image: { src: "/images/hero/trail-lavender.png", alt: "بطری ورزشی Trail یاسی", width: 1024, height: 1536 } },
      { id: "mint", name: "نعنایی", color: "#7dc5ae", price: "۱٬۵۹۰٬۰۰۰ تومان", badge: "انتخاب نویرا", href: "/products/trail-mint", isAvailable: true, image: { src: "/images/hero/trail-mint.png", alt: "بطری ورزشی Trail سبز نعنایی", width: 1024, height: 1536 } },
      { id: "yellow", name: "آفتابی", color: "#e9be3f", price: "۱٬۵۹۰٬۰۰۰ تومان", href: "/products/trail-yellow", isAvailable: false, image: { src: "/images/hero/trail-yellow.png", alt: "بطری ورزشی Trail زرد آفتابی", width: 1024, height: 1536 } },
    ],
  },
];

