"use client";

import Image from "next/image";
import { ImageOff } from "lucide-react";
import { useState } from "react";
import type { PromoBannerImage as PromoImageData } from "./home-promo-banner-grid.types";
import styles from "./HomePromoBannerGrid.module.scss";

export function PromoBannerImage({ image }: { image: PromoImageData }) {
  const [failed, setFailed] = useState(false);
  if (failed) return <span className={styles.imageFallback} role="img" aria-label={`${image.alt} — تصویر در دسترس نیست`}><ImageOff aria-hidden="true" /></span>;
  return <Image className={styles.productImage} src={image.src} alt={image.alt} width={image.width} height={image.height} sizes="(min-width: 1200px) 280px, (min-width: 768px) 44vw, 72vw" onError={() => setFailed(true)} />;
}

