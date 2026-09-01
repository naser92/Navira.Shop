import {
  Baby,
  BookOpen,
  CookingPot,
  Dumbbell,
  Gamepad2,
  Gift,
  Grid3X3,
  Laptop,
  PenLine,
  Shirt,
  Sparkles,
  type LucideIcon,
} from "lucide-react";
import type { CategoryIconKey, CategoryThemeKey } from "./home-category-strip.types";

export const categoryIconRegistry: Record<CategoryIconKey, LucideIcon> = {
  laptop: Laptop,
  shirt: Shirt,
  "cooking-pot": CookingPot,
  dumbbell: Dumbbell,
  pen: PenLine,
  sparkles: Sparkles,
  baby: Baby,
  gamepad: Gamepad2,
  book: BookOpen,
  gift: Gift,
  grid: Grid3X3,
};

export const categoryThemeClass: Record<CategoryThemeKey, string> = {
  blue: "themeBlue",
  purple: "themePurple",
  orange: "themeOrange",
  green: "themeGreen",
  teal: "themeTeal",
  pink: "themePink",
  yellow: "themeYellow",
  indigo: "themeIndigo",
};

