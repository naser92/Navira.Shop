export type PromoBannerImage = { src: string; mobileSrc?: string; alt: string; width: number; height: number };
export type PromoBannerThemeKey = "rose" | "ocean" | "violet" | "orange" | "sage";
export type PromoBannerLayout = "content-start-image-end" | "image-start-content-end" | "content-top-image-bottom" | "centered-overlay";

export interface HomePromoBanner {
  id: string;
  eyebrow?: string;
  title: string;
  description?: string;
  badgeText?: string;
  discountText?: string;
  image: PromoBannerImage;
  logo?: PromoBannerImage;
  href: string;
  ctaLabel: string;
  themeKey: PromoBannerThemeKey;
  layout: PromoBannerLayout;
  sortOrder: number;
  isActive: boolean;
  openInNewTab?: boolean;
}

export interface HomePromoBannerGridProps { banners: HomePromoBanner[]; title?: string; description?: string }

