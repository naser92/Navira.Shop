export const categoryIconKeys = [
  "laptop", "shirt", "cooking-pot", "dumbbell", "pen", "sparkles",
  "baby", "gamepad", "book", "gift", "grid",
] as const;

export type CategoryIconKey = (typeof categoryIconKeys)[number];
export type CategoryThemeKey = "blue" | "purple" | "orange" | "green" | "teal" | "pink" | "yellow" | "indigo";

export type CategoryIconSource =
  | { type: "icon"; iconKey: CategoryIconKey }
  | { type: "image"; src: string; alt: string; width: number; height: number };

export interface HomeCategoryItemData {
  id: string;
  slug: string;
  title: string;
  href: string;
  icon: CategoryIconSource;
  themeKey: CategoryThemeKey;
  badgeText?: string;
  sortOrder: number;
  isActive: boolean;
  isFeatured?: boolean;
}

export interface HomeCategoryStripProps {
  categories: HomeCategoryItemData[];
  title?: string;
  showAllItem?: { title: string; href: string };
}

