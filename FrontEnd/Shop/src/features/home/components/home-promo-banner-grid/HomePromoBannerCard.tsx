import Image from "next/image";
import Link from "next/link";
import { ArrowUpLeft } from "lucide-react";
import { PromoBannerImage } from "./PromoBannerImage";
import type { HomePromoBanner } from "./home-promo-banner-grid.types";
import { promoLayoutClasses, promoThemeClasses } from "./home-promo-banner-grid.utils";
import styles from "./HomePromoBannerGrid.module.scss";

export function HomePromoBannerCard({ banner }: { banner: HomePromoBanner }) {
  const className = [styles.card, styles[promoThemeClasses[banner.themeKey]], styles[promoLayoutClasses[banner.layout]]].filter(Boolean).join(" ");
  const externalProps = banner.openInNewTab ? { target: "_blank", rel: "noopener noreferrer" } : {};

  return (
    <Link className={className} href={banner.href} {...externalProps} aria-label={`${banner.title} — ${banner.ctaLabel}${banner.openInNewTab ? "، باز شدن در پنجره جدید" : ""}`}>
      <span className={styles.glow} aria-hidden="true" />
      <span className={styles.content}>
        <span className={styles.meta}>
          {banner.badgeText && <span className={styles.badge}>{banner.badgeText}</span>}
          {banner.eyebrow && <span className={styles.eyebrow}>{banner.eyebrow}</span>}
        </span>
        {banner.logo && <Image className={styles.logo} src={banner.logo.src} alt={banner.logo.alt} width={banner.logo.width} height={banner.logo.height} />}
        <strong className={styles.cardTitle}>{banner.title}</strong>
        {banner.description && <span className={styles.description}>{banner.description}</span>}
        {banner.discountText && <span className={styles.discount}>{banner.discountText}</span>}
        <span className={styles.cta}>{banner.ctaLabel}<ArrowUpLeft size={16} aria-hidden="true" /></span>
      </span>
      <span className={styles.visual}><PromoBannerImage image={banner.image} /></span>
    </Link>
  );
}
