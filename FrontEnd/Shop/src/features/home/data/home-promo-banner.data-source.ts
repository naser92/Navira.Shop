import { homePromoBannersMock } from "@/mocks/home-promo-banners.mock";
import type { HomePromoBanner } from "../components/home-promo-banner-grid";

export interface HomePromoBannerDataSource { getActiveBanners(): Promise<HomePromoBanner[]> }
export class MockHomePromoBannerDataSource implements HomePromoBannerDataSource {
  async getActiveBanners() { return homePromoBannersMock.filter((banner) => banner.isActive).sort((a, b) => a.sortOrder - b.sortOrder).slice(0, 4); }
}
export const homePromoBannerDataSource: HomePromoBannerDataSource = new MockHomePromoBannerDataSource();

