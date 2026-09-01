export interface HeroVariant {
  id: string;
  name: string;
  color: string;
  price: string;
  image: { src: string; alt: string; width: number; height: number };
  badge?: string;
  href: string;
  isAvailable: boolean;
}

export interface HeroSlide {
  id: string;
  eyebrow: string;
  title: string;
  description?: string;
  model: string;
  sortOrder: number;
  isActive: boolean;
  theme: {
    background: string;
    surface: string;
    accent: string;
    ink: string;
    glow: string;
  };
  variants: HeroVariant[];
}

export interface HeroSliderConfig {
  autoplayMs?: number;
  loop?: boolean;
}

