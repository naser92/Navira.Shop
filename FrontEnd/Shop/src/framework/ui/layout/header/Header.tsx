// Server Component: the header markup itself needs no client-only APIs.
// Interactive pieces (search input, icon buttons) rely only on native HTML
// behavior, so no "use client" directive is required here
// (docs/ARCHITECTURE.md section 7).

import { LogIn, Search, ShoppingBag } from "lucide-react";

import { PageContainer } from "@/framework/ui/layout/PageContainer";

import { NaviraLogo } from "./NaviraLogo";
import styles from "./Header.module.scss";

// TODO: replace with the real auth state once the `auth` feature ships.
const isAuthenticated = false;

export function Header() {
  return (
    <header className={styles.header}>
      <div className={styles.bar}>
        <PageContainer className={styles.inner}>
          <div className={styles.brand}>
            <span className={styles.brandIcon}>
              <NaviraLogo />
            </span>
            <span className={styles.brandName}>NaviraShop</span>
          </div>

          <div className={styles.searchArea}>
            <form
              className={styles.searchField}
              role="search"
              aria-label="جستجوی محصولات"
            >
              <span className={styles.searchIcon}>
                <Search size={18} strokeWidth={2} aria-hidden="true" />
              </span>
              <input
                type="search"
                name="search"
                placeholder="جستجو در نویرا شاپ..."
                aria-label="جستجو در نویرا شاپ"
                className={styles.searchInput}
              />
            </form>
          </div>

          <div className={styles.actions}>
            <button
              type="button"
              className={`${styles.iconButton} ${styles.cartButton}`}
              aria-label="سبد خرید"
            >
              <ShoppingBag size={20} strokeWidth={2} aria-hidden="true" />
            </button>

            {isAuthenticated ? null : (
              <button
                type="button"
                className={styles.authButton}
                aria-label="ورود | ثبت‌نام"
              >
                <LogIn size={18} strokeWidth={2} aria-hidden="true" />
                <span className={styles.authButtonLabel} aria-hidden="true">
                  ورود | ثبت‌نام
                </span>
              </button>
            )}
          </div>
        </PageContainer>
      </div>
    </header>
  );
}
