import { Grid3X3 } from "lucide-react";
import Link from "next/link";
import { HomeCategoryItem } from "./HomeCategoryItem";
import type { HomeCategoryStripProps } from "./home-category-strip.types";
import styles from "./HomeCategoryStrip.module.scss";

export function HomeCategoryStrip({ categories, title = "دسته‌بندی‌های فروشگاه", showAllItem }: HomeCategoryStripProps) {
  const activeCategories = categories.filter((category) => category.isActive).sort((a, b) => a.sortOrder - b.sortOrder);
  if (activeCategories.length === 0) return null;

  return (
    <section className={styles.section} aria-labelledby="home-categories-title">
      <h2 id="home-categories-title" className={styles.title}>{title}</h2>
      <div className={styles.scrollFrame}>
        <ul className={styles.list}>
          {activeCategories.map((category) => <HomeCategoryItem key={category.id} category={category} />)}
          {showAllItem && (
            <li className={styles.item}>
              <Link className={`${styles.link} ${styles.showAll}`} href={showAllItem.href}>
                <span className={styles.circle} aria-hidden="true"><Grid3X3 size={31} strokeWidth={1.8} /></span>
                <span className={styles.label}>{showAllItem.title}</span>
              </Link>
            </li>
          )}
        </ul>
      </div>
    </section>
  );
}

