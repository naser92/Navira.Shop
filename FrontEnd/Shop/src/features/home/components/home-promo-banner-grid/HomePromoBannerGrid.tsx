import { HomePromoBannerCard } from "./HomePromoBannerCard";
import type { HomePromoBannerGridProps } from "./home-promo-banner-grid.types";
import styles from "./HomePromoBannerGrid.module.scss";

export function HomePromoBannerGrid({ banners, title = "پیشنهادهای ویژه فروشگاه", description }: HomePromoBannerGridProps) {
  const activeBanners = banners.filter((banner) => banner.isActive).sort((a, b) => a.sortOrder - b.sortOrder).slice(0, 4);
  if (activeBanners.length === 0) return null;
  return (
    <section className={styles.section} aria-labelledby="promo-banners-title">
      <header className={styles.header}>
        <h2 id="promo-banners-title">{title}</h2>
        {description && <p>{description}</p>}
      </header>
      <ul className={styles.grid}>
        {activeBanners.map((banner) => <li key={banner.id}><HomePromoBannerCard banner={banner} /></li>)}
      </ul>
    </section>
  );
}

