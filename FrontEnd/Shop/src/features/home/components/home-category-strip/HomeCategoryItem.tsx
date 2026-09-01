import Image from "next/image";
import Link from "next/link";
import { CircleHelp } from "lucide-react";
import type { HomeCategoryItemData } from "./home-category-strip.types";
import { categoryIconRegistry, categoryThemeClass } from "./home-category-strip.utils";
import styles from "./HomeCategoryStrip.module.scss";

export function HomeCategoryItem({ category }: { category: HomeCategoryItemData }) {
  const themeClass = styles[categoryThemeClass[category.themeKey]] ?? styles.themeBlue;
  let visual;

  if (category.icon.type === "image") {
    visual = <Image src={category.icon.src} alt="" width={category.icon.width} height={category.icon.height} sizes="76px" />;
  } else {
    const Icon = categoryIconRegistry[category.icon.iconKey] ?? CircleHelp;
    visual = <Icon size={32} strokeWidth={1.8} aria-hidden="true" />;
  }

  return (
    <li className={styles.item}>
      <Link className={`${styles.link} ${themeClass}`} href={category.href} data-featured={category.isFeatured || undefined}>
        <span className={styles.circle} aria-hidden="true">{visual}</span>
        <span className={styles.label}>{category.title}</span>
        {category.badgeText && <span className={styles.badge}>{category.badgeText}</span>}
      </Link>
    </li>
  );
}
