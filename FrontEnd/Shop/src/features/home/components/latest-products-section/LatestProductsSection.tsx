import Link from "next/link";
import { ArrowLeft } from "lucide-react";
import { LatestProductsRail } from "./LatestProductsRail";
import type { LatestProductsSectionProps } from "./latest-products.types";
import styles from "./LatestProductsSection.module.scss";

export function LatestProductsSection({ products, title, description, viewAllHref, state = "ready" }: LatestProductsSectionProps) {
  const activeProducts = products.filter((product) => product.isActive && product.variants.length > 0);
  return <section className={styles.section} aria-labelledby="latest-products-title">
    <header className={styles.header}>
      <div><h2 id="latest-products-title">{title}</h2>{description && <p>{description}</p>}</div>
      <Link href={viewAllHref}>مشاهده همه <ArrowLeft size={17} /></Link>
    </header>
    {state === "error" ? <div className={styles.message}><p>نمایش محصولات جدید فعلاً ممکن نیست.</p><Link href={viewAllHref}>مشاهده همه محصولات</Link></div>
      : activeProducts.length === 0 ? <div className={styles.message}><p>به‌زودی محصولات جدیدی اضافه می‌کنیم.</p><Link href={viewAllHref}>مشاهده همه محصولات</Link></div>
      : <LatestProductsRail products={activeProducts} />}
  </section>;
}

export function LatestProductsSkeleton() {
  return <section className={styles.section} aria-label="در حال بارگذاری جدیدترین محصولات" aria-busy="true"><div className={styles.skeletonHeader} /> <div className={styles.skeletons}>{[0,1,2].map((item) => <div key={item} />)}</div></section>;
}

