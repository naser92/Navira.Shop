"use client";

import { ChevronLeft, ChevronRight } from "lucide-react";
import { useRef } from "react";
import type { LatestProduct } from "./latest-products.types";
import { ProductCard } from "./ProductCard";
import styles from "./LatestProductsSection.module.scss";

export function LatestProductsRail({ products }: { products: LatestProduct[] }) {
  const railRef = useRef<HTMLUListElement>(null);
  const visibleIndex = useRef(0);
  const move = (step: number) => {
    const items = railRef.current?.children;
    if (!items?.length) return;
    visibleIndex.current = Math.min(items.length - 1, Math.max(0, visibleIndex.current + step));
    items[visibleIndex.current]?.scrollIntoView({ behavior: "smooth", block: "nearest", inline: "start" });
  };
  return <div className={styles.railFrame}>
    {products.length > 4 && <nav className={styles.navigation} aria-label="پیمایش جدیدترین محصولات">
      <button type="button" onClick={() => move(-1)} aria-label="محصولات قبلی"><ChevronRight /></button>
      <button type="button" onClick={() => move(1)} aria-label="محصولات بعدی"><ChevronLeft /></button>
    </nav>}
    <ul ref={railRef} className={`${styles.rail} ${products.length === 3 ? styles.threeProducts : ""}`}>
      {products.map((product) => <li key={product.id}><ProductCard product={product} /></li>)}
    </ul>
  </div>;
}
