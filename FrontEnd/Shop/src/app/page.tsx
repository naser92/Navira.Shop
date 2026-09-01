import { HeroProductSlider, HomeCategoryStrip, heroSliderDataSource, homeCategoryDataSource } from "@/features/home";
import styles from "./page.module.scss";

export default async function HomePage() {
  const [slides, categories] = await Promise.all([
    heroSliderDataSource.getActiveSlides(),
    homeCategoryDataSource.getFeaturedCategories(),
  ]);

  return (
    <main className={styles.main}>
      <HeroProductSlider slides={slides} config={{ autoplayMs: 6500, loop: true }} />
      <HomeCategoryStrip categories={categories} showAllItem={{ title: "مشاهده همه", href: "/categories" }} />
    </main>
  );
}
