"use client";

import Image from "next/image";
import Link from "next/link";
import { ChevronLeft, ChevronRight, ShoppingBag } from "lucide-react";
import {
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState,
  type CSSProperties,
  type FocusEvent,
  type TouchEvent,
} from "react";

import { HeroModelText } from "./HeroModelText";
import type { HeroSlide, HeroSliderConfig } from "./hero-product-slider.types";
import { useHeroPointerEffect } from "./useHeroPointerEffect";
import styles from "./HeroProductSlider.module.scss";

type HeroCssProperties = CSSProperties & Record<`--${string}`, string>;

interface HeroProductSliderProps {
  slides: HeroSlide[];
  config?: HeroSliderConfig;
}

export function HeroProductSlider({ slides, config }: HeroProductSliderProps) {
  const autoplayMs = config?.autoplayMs ?? 6500;
  const loop = config?.loop ?? true;
  const rootRef = useRef<HTMLElement>(null);
  const touchStartX = useRef<number | null>(null);
  const transitionTimer = useRef<ReturnType<typeof setTimeout> | null>(null);
  const pauseTimer = useRef<ReturnType<typeof setTimeout> | null>(null);
  const [slideIndex, setSlideIndex] = useState(0);
  const [variantIds, setVariantIds] = useState<Record<string, string>>({});
  const [isHovered, setIsHovered] = useState(false);
  const [isFocused, setIsFocused] = useState(false);
  const [isVisible, setIsVisible] = useState(true);
  const [isInteracting, setIsInteracting] = useState(false);
  const [isTransitioning, setIsTransitioning] = useState(false);
  const [imageFailed, setImageFailed] = useState(false);
  const [autoplayKey, setAutoplayKey] = useState(0);

  useHeroPointerEffect(rootRef);

  const activeSlide = slides[slideIndex] ?? slides[0];
  const selectedVariant = activeSlide?.variants.find((item) => item.id === variantIds[activeSlide.id])
    ?? activeSlide?.variants.find((item) => item.isAvailable)
    ?? activeSlide?.variants[0];

  const themeStyle = useMemo<HeroCssProperties>(() => ({
    "--hero-bg": activeSlide?.theme.background ?? "#eef2f6",
    "--hero-surface": activeSlide?.theme.surface ?? "#ffffff",
    "--hero-accent": activeSlide?.theme.accent ?? "#2f6fed",
    "--hero-ink": activeSlide?.theme.ink ?? "#172033",
    "--hero-glow": activeSlide?.theme.glow ?? "#dce8ff",
  }), [activeSlide]);

  const pauseTemporarily = useCallback(() => {
    setIsInteracting(true);
    setAutoplayKey((value) => value + 1);
    if (pauseTimer.current) clearTimeout(pauseTimer.current);
    pauseTimer.current = setTimeout(() => setIsInteracting(false), 4000);
  }, []);

  const goTo = useCallback((nextIndex: number) => {
    if (slides.length < 2 || isTransitioning) return;
    let normalized = nextIndex;
    if (loop) normalized = (nextIndex + slides.length) % slides.length;
    else normalized = Math.min(slides.length - 1, Math.max(0, nextIndex));
    if (normalized === slideIndex) return;
    setIsTransitioning(true);
    setImageFailed(false);
    setSlideIndex(normalized);
    setAutoplayKey((value) => value + 1);
    if (transitionTimer.current) clearTimeout(transitionTimer.current);
    transitionTimer.current = setTimeout(() => setIsTransitioning(false), 520);
  }, [isTransitioning, loop, slideIndex, slides.length]);

  useEffect(() => {
    const onVisibilityChange = () => setIsVisible(document.visibilityState === "visible");
    document.addEventListener("visibilitychange", onVisibilityChange);
    return () => document.removeEventListener("visibilitychange", onVisibilityChange);
  }, []);

  useEffect(() => {
    if (slides.length < 2 || isHovered || isFocused || isInteracting || !isVisible) return;
    const timer = window.setTimeout(() => goTo(slideIndex + 1), autoplayMs);
    return () => window.clearTimeout(timer);
  }, [autoplayKey, autoplayMs, goTo, isFocused, isHovered, isInteracting, isVisible, slideIndex, slides.length]);

  useEffect(() => {
    setImageFailed(false);
    const sources = slides.flatMap((slide) => slide.variants.map((variant) => variant.image.src));
    const preload = () => sources.forEach((src) => { const image = new window.Image(); image.src = src; });
    const idleWindow = window as Window & {
      requestIdleCallback?: (callback: () => void) => number;
      cancelIdleCallback?: (id: number) => void;
    };
    const idleId = idleWindow.requestIdleCallback
      ? idleWindow.requestIdleCallback(preload)
      : idleWindow.setTimeout(preload, 600);
    return () => {
      if (idleWindow.cancelIdleCallback) idleWindow.cancelIdleCallback(idleId);
      else idleWindow.clearTimeout(idleId);
    };
  }, [selectedVariant?.id, slides]);

  useEffect(() => () => {
    if (transitionTimer.current) clearTimeout(transitionTimer.current);
    if (pauseTimer.current) clearTimeout(pauseTimer.current);
  }, []);

  if (!activeSlide || !selectedVariant) {
    return <section className={styles.empty}><h1>محصولات ویژه به‌زودی می‌رسند</h1></section>;
  }

  const selectVariant = (id: string) => {
    const variant = activeSlide.variants.find((item) => item.id === id);
    if (!variant?.isAvailable) return;
    setVariantIds((current) => ({ ...current, [activeSlide.id]: id }));
    setImageFailed(false);
    pauseTemporarily();
  };
  const onFocusOut = (event: FocusEvent<HTMLElement>) => {
    if (!event.currentTarget.contains(event.relatedTarget)) setIsFocused(false);
  };
  const onTouchEnd = (event: TouchEvent<HTMLElement>) => {
    if (touchStartX.current === null) return;
    const distance = event.changedTouches[0]?.clientX ?? touchStartX.current;
    const delta = distance - touchStartX.current;
    if (Math.abs(delta) > 48) goTo(slideIndex + (delta < 0 ? 1 : -1));
    touchStartX.current = null;
    pauseTemporarily();
  };

  return (
    <section
      ref={rootRef}
      className={styles.hero}
      style={themeStyle}
      aria-roledescription="carousel"
      aria-label="محصولات منتخب نویرا"
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
      onFocusCapture={() => setIsFocused(true)}
      onBlurCapture={onFocusOut}
      onTouchStart={(event) => { touchStartX.current = event.touches[0]?.clientX ?? null; }}
      onTouchEnd={onTouchEnd}
      onKeyDown={(event) => {
        if (event.key === "ArrowLeft") goTo(slideIndex + 1);
        if (event.key === "ArrowRight") goTo(slideIndex - 1);
        if (event.key === "Home") goTo(0);
        if (event.key === "End") goTo(slides.length - 1);
      }}
    >
      <div className={styles.glow} />
      <HeroModelText value={activeSlide.model} />

      <div className={styles.content} key={activeSlide.id}>
        <p className={styles.eyebrow}>{activeSlide.eyebrow}</p>
        <h1>{activeSlide.title}</h1>
        {activeSlide.description && <p className={styles.description}>{activeSlide.description}</p>}
        <div className={styles.priceRow}>
          <strong>{selectedVariant.price}</strong>
          {selectedVariant.badge && <span>{selectedVariant.badge}</span>}
        </div>
        <Link className={styles.cta} href={selectedVariant.href} aria-disabled={!selectedVariant.isAvailable}>
          <ShoppingBag size={18} aria-hidden="true" /> مشاهده محصول
        </Link>
      </div>

      <div className={styles.productStage}>
        <div className={styles.productHalo} />
        {imageFailed ? (
          <div className={styles.imageFallback} role="img" aria-label={selectedVariant.image.alt}>
            تصویر محصول در دسترس نیست
          </div>
        ) : (
          <Image
            key={selectedVariant.id}
            className={styles.productImage}
            src={selectedVariant.image.src}
            alt={selectedVariant.image.alt}
            width={selectedVariant.image.width}
            height={selectedVariant.image.height}
            priority={slideIndex === 0}
            sizes="(min-width: 1024px) 38vw, (min-width: 640px) 48vw, 78vw"
            onError={() => setImageFailed(true)}
          />
        )}
      </div>

      <fieldset className={styles.palette} aria-label="انتخاب رنگ محصول">
        <legend>رنگ: <strong>{selectedVariant.name}</strong></legend>
        <div>
          {activeSlide.variants.map((variant) => (
            <label key={variant.id} className={styles.swatchLabel} title={variant.isAvailable ? variant.name : `${variant.name} — ناموجود`}>
              <input
                type="radio"
                name={`variant-${activeSlide.id}`}
                value={variant.id}
                checked={variant.id === selectedVariant.id}
                disabled={!variant.isAvailable}
                onChange={() => selectVariant(variant.id)}
              />
              <span style={{ "--swatch": variant.color } as HeroCssProperties} />
              <b>{variant.name}</b>
            </label>
          ))}
        </div>
      </fieldset>

      {slides.length > 1 && (
        <div className={styles.navigation}>
          <button type="button" onClick={() => { goTo(slideIndex - 1); pauseTemporarily(); }} aria-label="اسلاید قبلی"><ChevronRight /></button>
          <div className={styles.pagination} aria-label="انتخاب اسلاید">
            {slides.map((slide, index) => (
              <button key={slide.id} type="button" className={index === slideIndex ? styles.activeDot : undefined} onClick={() => { goTo(index); pauseTemporarily(); }} aria-label={`اسلاید ${index + 1}`} aria-current={index === slideIndex ? "true" : undefined} />
            ))}
          </div>
          <button type="button" onClick={() => { goTo(slideIndex + 1); pauseTemporarily(); }} aria-label="اسلاید بعدی"><ChevronLeft /></button>
        </div>
      )}
      <p className={styles.liveRegion} aria-live="polite">{activeSlide.title}، رنگ {selectedVariant.name}</p>
    </section>
  );
}
