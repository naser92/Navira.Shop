export interface ProductImage { src: string; alt: string; width: number; height: number }
export interface ProductColorVariant {
  id: string; sku?: string; colorName: string; swatchColor: string; image: ProductImage;
  price: number; oldPrice?: number; isAvailable: boolean; href: string;
}
export interface LatestProduct {
  id: string; slug: string; brandName?: string; title: string; defaultVariantId: string;
  variants: ProductColorVariant[]; badgeText?: string; rating?: number; reviewCount?: number;
  createdAt: string; isFavorite?: boolean; isActive: boolean;
}
export interface LatestProductsSectionProps {
  products: LatestProduct[]; title: string; description?: string; viewAllHref: string; state?: "ready" | "error";
}

