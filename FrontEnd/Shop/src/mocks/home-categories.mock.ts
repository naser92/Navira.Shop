import type { HomeCategoryItemData } from "@/features/home/components/home-category-strip";

export const homeCategoriesMock: HomeCategoryItemData[] = [
  { id: "digital", slug: "digital-products", title: "کالای دیجیتال", href: "/categories/digital-products", icon: { type: "icon", iconKey: "laptop" }, themeKey: "blue", sortOrder: 1, isActive: true, isFeatured: true },
  { id: "fashion", slug: "fashion", title: "مد و پوشاک", href: "/categories/fashion", icon: { type: "icon", iconKey: "shirt" }, themeKey: "purple", sortOrder: 2, isActive: true },
  { id: "home-kitchen", slug: "home-and-kitchen", title: "خانه و آشپزخانه", href: "/categories/home-and-kitchen", icon: { type: "icon", iconKey: "cooking-pot" }, themeKey: "orange", sortOrder: 3, isActive: true, badgeText: "محبوب" },
  { id: "sports", slug: "sports", title: "ورزش و سفر", href: "/categories/sports", icon: { type: "icon", iconKey: "dumbbell" }, themeKey: "green", sortOrder: 4, isActive: true },
  { id: "stationery", slug: "stationery", title: "لوازم‌التحریر", href: "/categories/stationery", icon: { type: "icon", iconKey: "pen" }, themeKey: "teal", sortOrder: 5, isActive: true },
  { id: "beauty", slug: "beauty-health", title: "زیبایی و سلامت", href: "/categories/beauty-health", icon: { type: "icon", iconKey: "sparkles" }, themeKey: "pink", sortOrder: 6, isActive: true },
  { id: "kids", slug: "kids", title: "کودک و نوزاد", href: "/categories/kids", icon: { type: "icon", iconKey: "baby" }, themeKey: "yellow", sortOrder: 7, isActive: true, badgeText: "جدید" },
  { id: "gaming", slug: "gaming", title: "بازی و سرگرمی", href: "/categories/gaming", icon: { type: "icon", iconKey: "gamepad" }, themeKey: "indigo", sortOrder: 8, isActive: true },
  { id: "books", slug: "books", title: "کتاب و فرهنگ", href: "/categories/books", icon: { type: "icon", iconKey: "book" }, themeKey: "orange", sortOrder: 9, isActive: true },
  { id: "gifts", slug: "gifts", title: "هدیه‌های نویرا", href: "/categories/gifts", icon: { type: "icon", iconKey: "gift" }, themeKey: "pink", sortOrder: 10, isActive: true },
];

