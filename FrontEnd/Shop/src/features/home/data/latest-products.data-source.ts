import { latestProductsMock } from "@/mocks/latest-products.mock";
import type { LatestProduct } from "../components/latest-products-section";

export interface LatestProductsDataSource { getLatestProducts(limit: number): Promise<LatestProduct[]> }
export class MockLatestProductsDataSource implements LatestProductsDataSource {
  async getLatestProducts(limit: number) {
    const safeLimit = Math.min(12, Math.max(3, limit));
    return latestProductsMock.filter((product) => product.isActive).sort((a, b) => Date.parse(b.createdAt) - Date.parse(a.createdAt)).slice(0, safeLimit);
  }
}
export const latestProductsDataSource: LatestProductsDataSource = new MockLatestProductsDataSource();

