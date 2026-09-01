"use client";

import { useEffect, useId, useRef, useState } from "react";
import Link from "next/link";
import { ChevronDown, Heart, LogIn, Menu, Search, ShoppingBag, X } from "lucide-react";
import { mockHeaderData } from "@/mocks/storefront-layout.mock";
import { PageContainer } from "../PageContainer";
import type { HeaderViewModel } from "../types";
import { NaviraLogo } from "./NaviraLogo";
import styles from "./Header.module.scss";

interface HeaderProps { data?: HeaderViewModel; }

export function Header({ data = mockHeaderData }: HeaderProps) {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const drawerTitleId = useId();
  const closeButtonRef = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    if (!isMenuOpen) return;
    const previousOverflow = document.body.style.overflow;
    document.body.style.overflow = "hidden";
    closeButtonRef.current?.focus();
    const handleEscape = (event: KeyboardEvent) => event.key === "Escape" && setIsMenuOpen(false);
    document.addEventListener("keydown", handleEscape);
    return () => {
      document.body.style.overflow = previousOverflow;
      document.removeEventListener("keydown", handleEscape);
    };
  }, [isMenuOpen]);

  return (
    <header className={styles.header}>
      <div className={styles.utilityBar}>ارسال رایگان برای سفارش‌های بالای ۱٬۵۰۰٬۰۰۰ تومان</div>
      <div className={styles.bar}>
        <PageContainer className={styles.inner}>
          <button type="button" className={`${styles.iconButton} ${styles.menuButton}`} aria-label="باز کردن منوی اصلی" aria-expanded={isMenuOpen} onClick={() => setIsMenuOpen(true)}><Menu size={22} aria-hidden="true" /></button>
          <Link href="/" className={styles.brand} aria-label="صفحه اصلی نویرا شاپ"><span className={styles.brandIcon}><NaviraLogo /></span><span className={styles.brandName}>NaviraShop</span></Link>
          <form className={styles.searchField} role="search" action="/products">
            <Search size={18} className={styles.searchIcon} aria-hidden="true" />
            <label className={styles.srOnly} htmlFor="store-search">جستجوی محصولات</label>
            <input id="store-search" type="search" name="q" placeholder="جست‌وجوی ماگ، طرح یا رنگ..." className={styles.searchInput} />
          </form>
          <div className={styles.actions}>
            <Link href="/favorites" className={`${styles.iconButton} ${styles.favoriteButton}`} aria-label="علاقه‌مندی‌ها"><Heart size={20} aria-hidden="true" /></Link>
            <Link href="/cart" className={`${styles.iconButton} ${styles.cartButton}`} aria-label={`سبد خرید، ${data.cartItemCount} کالا`}><ShoppingBag size={20} aria-hidden="true" />{data.cartItemCount > 0 && <span className={styles.cartBadge}>{data.cartItemCount.toLocaleString("fa-IR")}</span>}</Link>
            <Link href="/login" className={styles.authButton}><LogIn size={18} aria-hidden="true" /><span>{data.userDisplayName ?? "ورود | ثبت‌نام"}</span></Link>
          </div>
        </PageContainer>
      </div>
      <nav className={styles.desktopNav} aria-label="منوی اصلی"><PageContainer><ul className={styles.navList}>{data.navigation.map((item, index) => (
        <li key={item.href} className={styles.navItem}>
          <Link href={item.href} className={index === 0 ? styles.activeNavLink : styles.navLink} aria-current={index === 0 ? "page" : undefined}>{item.label}{item.children && <ChevronDown size={15} aria-hidden="true" />}</Link>
          {item.children && <ul className={styles.submenu} aria-label={`زیرمنوی ${item.label}`}>{item.children.map((child) => <li key={child.href}><Link href={child.href}>{child.label}</Link></li>)}</ul>}
        </li>
      ))}</ul></PageContainer></nav>
      {isMenuOpen && <div className={styles.mobileLayer} role="presentation" onMouseDown={(event) => event.target === event.currentTarget && setIsMenuOpen(false)}>
        <aside className={styles.drawer} role="dialog" aria-modal="true" aria-labelledby={drawerTitleId}>
          <div className={styles.drawerHeader}><span id={drawerTitleId}>منوی نویرا</span><button ref={closeButtonRef} type="button" className={styles.iconButton} aria-label="بستن منو" onClick={() => setIsMenuOpen(false)}><X size={22} aria-hidden="true" /></button></div>
          <nav aria-label="منوی موبایل"><ul className={styles.mobileNavList}>{data.navigation.map((item) => <li key={item.href}><Link href={item.href} onClick={() => setIsMenuOpen(false)}>{item.label}</Link>{item.children && <ul>{item.children.map((child) => <li key={child.href}><Link href={child.href} onClick={() => setIsMenuOpen(false)}>{child.label}</Link></li>)}</ul>}</li>)}</ul></nav>
        </aside>
      </div>}
    </header>
  );
}
