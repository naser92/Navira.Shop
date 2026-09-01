import type { HomeCategoryItemData } from "../components/home-category-strip";
import { homeCategoriesMock } from "@/mocks/home-categories.mock";

export interface HomeCategoryDataSource {
  getFeaturedCategories(): Promise<HomeCategoryItemData[]>;
}

export class MockHomeCategoryDataSource implements HomeCategoryDataSource {
  async getFeaturedCategories() {
    return homeCategoriesMock.filter((category) => category.isActive).sort((a, b) => a.sortOrder - b.sortOrder);
  }
}

export const homeCategoryDataSource: HomeCategoryDataSource = new MockHomeCategoryDataSource();

