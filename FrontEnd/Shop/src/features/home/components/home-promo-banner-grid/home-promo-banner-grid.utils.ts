import type { PromoBannerLayout, PromoBannerThemeKey } from "./home-promo-banner-grid.types";

export const promoThemeClasses: Record<PromoBannerThemeKey, string> = {
  rose: "themeRose", ocean: "themeOcean", violet: "themeViolet", orange: "themeOrange", sage: "themeSage",
};

export const promoLayoutClasses: Record<PromoBannerLayout, string> = {
  "content-start-image-end": "contentStart",
  "image-start-content-end": "imageStart",
  "content-top-image-bottom": "contentTop",
  "centered-overlay": "centeredOverlay",
};

