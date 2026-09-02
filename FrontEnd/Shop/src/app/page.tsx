import { HeroProductSlider, HomeCategoryStrip, HomePromoBannerGrid, LatestProductsSection, heroSliderDataSource, homeCategoryDataSource, homePromoBannerDataSource, latestProductsDataSource } from "@/features/home";
import styles from "./page.module.scss";

export default async function HomePage() {
  const [slides, categories, promoBanners, latestProducts] = await Promise.all([
    heroSliderDataSource.getActiveSlides(),
    homeCategoryDataSource.getFeaturedCategories(),
    homePromoBannerDataSource.getActiveBanners(),
    latestProductsDataSource.getLatestProducts(8),
  ]);

  return (
    <main className={styles.main}>
      <HeroProductSlider slides={slides} config={{ autoplayMs: 6500, loop: true }} />
      <HomeCategoryStrip categories={categories} showAllItem={{ title: "مشاهده همه", href: "/categories" }} />
      <HomePromoBannerGrid banners={promoBanners} />
      <LatestProductsSection products={latestProducts} title="جدیدترین محصولات" description="تازه‌ترین انتخاب‌ها برای شما" viewAllHref="/products?sort=newest" />
    </main>
  );
}
