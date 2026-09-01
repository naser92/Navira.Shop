import { homeHeroSlidesMock } from "@/mocks/home-hero-slides.mock";
import type { HeroSlide } from "../components/hero-product-slider";

export interface HeroSliderDataSource { getActiveSlides(): Promise<HeroSlide[]>; }

class MockHeroSliderDataSource implements HeroSliderDataSource {
  async getActiveSlides() {
    return homeHeroSlidesMock.filter((slide) => slide.isActive && slide.variants.length > 0).sort((a, b) => a.sortOrder - b.sortOrder);
  }
}

export const heroSliderDataSource: HeroSliderDataSource = new MockHeroSliderDataSource();

