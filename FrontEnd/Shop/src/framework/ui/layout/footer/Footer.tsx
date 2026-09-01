import Link from "next/link";
import { Instagram, Mail, MapPin, Phone, Send, ShieldCheck, Truck } from "lucide-react";
import { mockFooterData } from "@/mocks/storefront-layout.mock";
import { PageContainer } from "../PageContainer";
import type { FooterViewModel } from "../types";
import { NaviraLogo } from "../header/NaviraLogo";
import styles from "./Footer.module.scss";

interface FooterProps { data?: FooterViewModel; }

export function Footer({ data = mockFooterData }: FooterProps) {
  return (
    <footer className={styles.footer}>
      <PageContainer>
        <section className={styles.newsletter} aria-labelledby="newsletter-title">
          <div><span className={styles.eyebrow}>نامه‌های نویرا</span><h2 id="newsletter-title">از تازه‌های دوست‌داشتنی باخبر شوید</h2><p>پیشنهادهای ویژه و مجموعه‌های تازه، بدون پیام‌های اضافه.</p></div>
          <form className={styles.newsletterForm}><label className={styles.srOnly} htmlFor="newsletter-email">ایمیل</label><div className={styles.emailField}><Mail size={18} aria-hidden="true" /><input id="newsletter-email" type="email" placeholder="example@email.com" required /></div><button type="submit"><Send size={18} aria-hidden="true" />عضویت</button></form>
        </section>
        <div className={styles.mainGrid}>
          <div className={styles.about}><Link href="/" className={styles.brand}><NaviraLogo /><span>NaviraShop</span></Link><p>{data.description}</p><div className={styles.socials}>{data.socialLinks.map((link) => <a key={link.label} href={link.href} target="_blank" rel="noreferrer" aria-label={link.label}>{link.label === "تلگرام" ? <Send size={19} aria-hidden="true" /> : <Instagram size={19} aria-hidden="true" />}</a>)}</div></div>
          {data.linkGroups.map((group) => <nav key={group.title} aria-label={group.title}><h2>{group.title}</h2><ul>{group.links.map((link) => <li key={link.href}><Link href={link.href}>{link.label}</Link></li>)}</ul></nav>)}
          <div className={styles.contact}><h2>ارتباط با نویرا</h2><address>{data.contacts.map((item) => <a key={item.label} href={item.href}>{item.label === "تلفن" ? <Phone size={17} aria-hidden="true" /> : <Mail size={17} aria-hidden="true" />}<span>{item.value}</span></a>)}<span><MapPin size={17} aria-hidden="true" />تهران، ایران</span></address></div>
        </div>
        <div className={styles.trustRow}>{data.trustBadges.map((badge, index) => <div key={badge.title}>{index === 1 ? <Truck size={22} aria-hidden="true" /> : <ShieldCheck size={22} aria-hidden="true" />}<span><strong>{badge.title}</strong><small>{badge.description}</small></span></div>)}</div>
        <div className={styles.legal}><p>© {new Date().getFullYear().toLocaleString("fa-IR", { useGrouping: false })} نویرا شاپ؛ همه حقوق محفوظ است.</p><div><Link href="/terms">قوانین استفاده</Link><Link href="/privacy">حریم خصوصی</Link></div></div>
      </PageContainer>
    </footer>
  );
}
