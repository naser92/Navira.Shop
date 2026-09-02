"use client";

import Image from "next/image";
import Link from "next/link";
import { Heart, ImageOff, ShoppingBag, Star } from "lucide-react";
import { useEffect, useState } from "react";
import type { LatestProduct } from "./latest-products.types";
import { formatProductPrice, getDiscountPercent } from "./latest-products.utils";
import styles from "./LatestProductsSection.module.scss";

export function ProductCard({ product }: { product: LatestProduct }) {
  const initialVariant = product.variants.find((variant) => variant.id === product.defaultVariantId && variant.isAvailable)
    ?? product.variants.find((variant) => variant.isAvailable) ?? product.variants[0];
  const [variantId, setVariantId] = useState(initialVariant?.id ?? "");
  const [imageFailed, setImageFailed] = useState(false);
  const variant = product.variants.find((item) => item.id === variantId) ?? initialVariant;

  useEffect(() => {
    if (!variant) return;
    const timer = window.setTimeout(() => product.variants.filter((item) => item.id !== variant.id).forEach((item) => { const image = new window.Image(); image.src = item.image.src; }), 500);
    return () => window.clearTimeout(timer);
  }, [product.variants, variant]);

  if (!variant) return null;
  const discount = getDiscountPercent(variant.price, variant.oldPrice);
  const visibleVariants = product.variants.slice(0, 4);
  const remaining = product.variants.length - visibleVariants.length;

  return (
    <article className={styles.card}>
      <div className={styles.imageFrame}>
        <Link href={variant.href} className={styles.imageLink} aria-label={`مشاهده ${product.title}، رنگ ${variant.colorName}`}>
          {imageFailed ? <span className={styles.imageFallback} role="img" aria-label={`${variant.image.alt} — تصویر در دسترس نیست`}><ImageOff /></span> : (
            <Image key={variant.id} className={styles.productImage} src={variant.image.src} alt={variant.image.alt} width={variant.image.width} height={variant.image.height} sizes="(min-width: 1200px) 260px, (min-width: 768px) 36vw, 72vw" onError={() => setImageFailed(true)} />
          )}
        </Link>
        {product.badgeText && <span className={styles.newBadge}>{product.badgeText}</span>}
        <button className={styles.favorite} type="button" aria-label={`افزودن ${product.title} به علاقه‌مندی‌ها؛ پس از ورود فعال می‌شود`} aria-pressed={product.isFavorite ?? false} disabled title="پس از ورود به حساب فعال می‌شود"><Heart size={19} /></button>
      </div>
      <div className={styles.cardBody}>
        <div className={styles.productMeta}>
          {product.brandName && <span>{product.brandName}</span>}
          {product.rating !== undefined && <span className={styles.rating}><Star size={13} fill="currentColor" /> {product.rating.toLocaleString("fa-IR")} <small>({(product.reviewCount ?? 0).toLocaleString("fa-IR")})</small></span>}
        </div>
        <Link className={styles.productTitle} href={variant.href}>{product.title}</Link>
        <div className={styles.priceBlock}>
          <strong>{formatProductPrice(variant.price)}</strong>
          {variant.oldPrice && <del aria-label={`قیمت قبلی ${formatProductPrice(variant.oldPrice)}`}>{formatProductPrice(variant.oldPrice)}</del>}
          {discount && <span>٪{discount.toLocaleString("fa-IR")} تخفیف</span>}
        </div>
        <fieldset className={styles.swatches}>
          <legend>رنگ انتخابی: {variant.colorName}</legend>
          {visibleVariants.map((item) => (
            <button key={item.id} type="button" style={{ "--swatch-color": item.swatchColor } as React.CSSProperties} aria-label={`${item.colorName}${item.isAvailable ? "" : "، ناموجود"}`} aria-pressed={item.id === variant.id} disabled={!item.isAvailable} onClick={() => { setVariantId(item.id); setImageFailed(false); }}><span /></button>
          ))}
          {remaining > 0 && <Link href={variant.href} aria-label={`${remaining.toLocaleString("fa-IR")} رنگ دیگر`}>+{remaining.toLocaleString("fa-IR")}</Link>}
        </fieldset>
        {variant.isAvailable ? <Link className={styles.quickAdd} href={variant.href}><ShoppingBag size={17} /> مشاهده و خرید</Link> : <span className={styles.unavailable}>فعلاً ناموجود</span>}
      </div>
    </article>
  );
}

