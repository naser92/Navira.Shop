import type { ElementType, ReactNode } from "react";

import styles from "./PageContainer.module.scss";

interface PageContainerProps {
  children: ReactNode;
  as?: ElementType;
  className?: string;
}

// Base layout primitive per docs/DESIGN_SYSTEM.md section 12.1.
// Enforces the shared max-width and responsive horizontal padding so pages
// never build ad-hoc containers.
export function PageContainer({ children, as: Tag = "div", className }: PageContainerProps) {
  const containerClassName = className ? `${styles.container} ${className}` : styles.container;

  return <Tag className={containerClassName}>{children}</Tag>;
}
