import type { LatestProduct } from "@/features/home/components/latest-products-section";

export const latestProductsMock: LatestProduct[] = [
  { id: "product-106", slug: "trail-sport-bottle", brandName: "Navira Active", title: "قمقمه ورزشی Trail سبک و مقاوم", defaultVariantId: "mint", badgeText: "جدید", rating: 4.9, reviewCount: 18, createdAt: "2026-09-01T14:00:00Z", isActive: true, variants: [
    { id: "mint", sku: "TRL-MNT", colorName: "نعنایی", swatchColor: "#7dc5ae", image: { src: "/images/hero/trail-mint.png", alt: "قمقمه Trail نعنایی", width: 1024, height: 1536 }, price: 1590000, oldPrice: 1780000, isAvailable: true, href: "/products/trail-sport-bottle?variant=mint" },
    { id: "lavender", sku: "TRL-LAV", colorName: "یاسی", swatchColor: "#a18ac0", image: { src: "/images/hero/trail-lavender.png", alt: "قمقمه Trail یاسی", width: 1024, height: 1536 }, price: 1590000, isAvailable: true, href: "/products/trail-sport-bottle?variant=lavender" },
    { id: "yellow", sku: "TRL-YLW", colorName: "آفتابی", swatchColor: "#e9be3f", image: { src: "/images/hero/trail-yellow.png", alt: "قمقمه Trail آفتابی", width: 1024, height: 1536 }, price: 1590000, isAvailable: false, href: "/products/trail-sport-bottle?variant=yellow" },
  ] },
  { id: "product-105", slug: "adventure-travel-mug", brandName: "Navira", title: "تراول ماگ Adventure", defaultVariantId: "rose", badgeText: "جدید", rating: 4.8, reviewCount: 42, createdAt: "2026-08-29T10:30:00Z", isActive: true, variants: [
    { id: "rose", sku: "ADV-ROS", colorName: "رز", swatchColor: "#c9697e", image: { src: "/images/hero/adventure-rose.png", alt: "تراول ماگ Adventure رز", width: 1024, height: 1536 }, price: 1890000, oldPrice: 2190000, isAvailable: true, href: "/products/adventure-travel-mug?variant=rose" },
    { id: "ocean", sku: "ADV-OCN", colorName: "اقیانوسی", swatchColor: "#2f86a6", image: { src: "/images/hero/adventure-ocean.png", alt: "تراول ماگ Adventure اقیانوسی", width: 1024, height: 1536 }, price: 1920000, isAvailable: true, href: "/products/adventure-travel-mug?variant=ocean" },
    { id: "sage", sku: "ADV-SAG", colorName: "سیج", swatchColor: "#86a48d", image: { src: "/images/hero/adventure-sage.png", alt: "تراول ماگ Adventure سیج", width: 1024, height: 1536 }, price: 1890000, isAvailable: true, href: "/products/adventure-travel-mug?variant=sage" },
  ] },
  { id: "product-104", slug: "ember-ceramic-mug", brandName: "Navira Home", title: "ماگ سرامیکی Ember با لعاب مات", defaultVariantId: "cream", rating: 4.7, reviewCount: 31, createdAt: "2026-08-25T08:00:00Z", isActive: true, variants: [
    { id: "cream", colorName: "کرم", swatchColor: "#e8d8bd", image: { src: "/images/hero/ember-cream.png", alt: "ماگ Ember کرم", width: 1024, height: 1536 }, price: 1290000, isAvailable: true, href: "/products/ember-ceramic-mug?variant=cream" },
    { id: "terracotta", colorName: "تراکوتا", swatchColor: "#b95839", image: { src: "/images/hero/ember-terracotta.png", alt: "ماگ Ember تراکوتا", width: 1024, height: 1536 }, price: 1390000, isAvailable: true, href: "/products/ember-ceramic-mug?variant=terracotta" },
    { id: "navy", colorName: "سرمه‌ای", swatchColor: "#213f57", image: { src: "/images/hero/ember-navy.png", alt: "ماگ Ember سرمه‌ای", width: 1024, height: 1536 }, price: 1390000, isAvailable: true, href: "/products/ember-ceramic-mug?variant=navy" },
  ] },
  { id: "product-103", slug: "double-wall-cup", brandName: "N Glass", title: "لیوان دوجداره مینیمال", defaultVariantId: "clear", badgeText: "جدید", createdAt: "2026-08-20T16:00:00Z", isActive: true, variants: [
    { id: "clear", colorName: "شفاف", swatchColor: "#dbe7ea", image: { src: "/images/hero/ember-cream.png", alt: "لیوان دوجداره روشن", width: 1024, height: 1536 }, price: 980000, oldPrice: 1100000, isAvailable: true, href: "/products/double-wall-cup?variant=clear" },
  ] },
  { id: "product-102", slug: "nomad-flask", brandName: "Navira Active", title: "فلاسک Nomad مناسب سفر", defaultVariantId: "navy", rating: 4.6, reviewCount: 12, createdAt: "2026-08-17T11:00:00Z", isActive: true, variants: [
    { id: "navy", colorName: "سرمه‌ای", swatchColor: "#213f57", image: { src: "/images/hero/adventure-ocean.png", alt: "فلاسک Nomad سرمه‌ای", width: 1024, height: 1536 }, price: 2450000, isAvailable: true, href: "/products/nomad-flask?variant=navy" },
    { id: "sage", colorName: "سبز سیج", swatchColor: "#86a48d", image: { src: "/images/hero/adventure-sage.png", alt: "فلاسک Nomad سیج", width: 1024, height: 1536 }, price: 2450000, isAvailable: true, href: "/products/nomad-flask?variant=sage" },
  ] },
  { id: "product-101", slug: "ember-gift-set", brandName: "Navira Gift", title: "ست هدیه دو عددی ماگ Ember", defaultVariantId: "terracotta", rating: 5, reviewCount: 8, createdAt: "2026-08-12T09:15:00Z", isActive: true, variants: [
    { id: "terracotta", colorName: "تراکوتا", swatchColor: "#b95839", image: { src: "/images/hero/ember-terracotta.png", alt: "ست هدیه ماگ Ember تراکوتا", width: 1024, height: 1536 }, price: 2490000, oldPrice: 2780000, isAvailable: true, href: "/products/ember-gift-set?variant=terracotta" },
    { id: "cream", colorName: "کرم", swatchColor: "#e8d8bd", image: { src: "/images/hero/ember-cream.png", alt: "ست هدیه ماگ Ember کرم", width: 1024, height: 1536 }, price: 2390000, isAvailable: true, href: "/products/ember-gift-set?variant=cream" },
  ] },
];

