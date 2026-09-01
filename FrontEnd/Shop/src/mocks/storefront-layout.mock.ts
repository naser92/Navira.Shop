import type { FooterViewModel, HeaderViewModel } from "@/framework/ui/layout/types";

export const mockHeaderData: HeaderViewModel = {
  cartItemCount: 3,
  navigation: [
    { label: "خانه", href: "/" },
    { label: "فروشگاه", href: "/products", children: [
      { label: "ماگ سرامیکی", href: "/products?category=ceramic" },
      { label: "ماگ سفری", href: "/products?category=travel" },
      { label: "ست هدیه", href: "/products?category=gift" },
    ] },
    { label: "پرفروش‌ها", href: "/products?sort=popular" },
    { label: "داستان نویرا", href: "/about" },
    { label: "تماس با ما", href: "/contact" },
  ],
};

export const mockFooterData: FooterViewModel = {
  description: "نویرا، همراه لحظه‌های گرم شما؛ مجموعه‌ای انتخاب‌شده از ماگ‌های کاربردی و هدیه‌های دوست‌داشتنی.",
  linkGroups: [
    { title: "دسترسی سریع", links: [{ label: "فروشگاه", href: "/products" }, { label: "پرفروش‌ها", href: "/products?sort=popular" }, { label: "درباره نویرا", href: "/about" }] },
    { title: "خدمات مشتریان", links: [{ label: "راهنمای خرید", href: "/help/buying-guide" }, { label: "پیگیری سفارش", href: "/orders/track" }, { label: "رویه بازگشت کالا", href: "/help/returns" }] },
    { title: "دسته‌بندی‌ها", links: [{ label: "ماگ سرامیکی", href: "/products?category=ceramic" }, { label: "ماگ سفری", href: "/products?category=travel" }, { label: "ست هدیه", href: "/products?category=gift" }] },
  ],
  contacts: [{ label: "تلفن", value: "۰۲۱-۸۸۷۷۶۶۵۵", href: "tel:+982188776655" }, { label: "ایمیل", value: "hello@navirashop.ir", href: "mailto:hello@navirashop.ir" }],
  socialLinks: [{ label: "اینستاگرام", href: "https://instagram.com" }, { label: "تلگرام", href: "https://t.me" }],
  trustBadges: [{ title: "خرید امن", description: "پرداخت مطمئن" }, { title: "ارسال سریع", description: "به سراسر ایران" }, { title: "ضمانت سلامت", description: "بسته‌بندی ایمن" }],
};
