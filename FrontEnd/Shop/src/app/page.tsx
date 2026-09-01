import { HeroProductSlider, heroSliderDataSource } from "@/features/home";
import styles from "./page.module.scss";

export default async function HomePage() {
  const slides = await heroSliderDataSource.getActiveSlides();

  return (
    <main className={styles.main}>
      <HeroProductSlider slides={slides} config={{ autoplayMs: 6500, loop: true }} />
    </main>
  );
}
