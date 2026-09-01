import type { Metadata } from "next";
import { Vazirmatn } from "next/font/google";

import { AppProviders } from "@/framework/providers";
import { Footer, Header } from "@/framework/ui/layout";
import "@/styles/globals.scss";

const vazirmatn = Vazirmatn({
  subsets: ["arabic", "latin"],
  variable: "--font-vazirmatn",
  display: "swap",
});

export const metadata: Metadata = {
  title: "NaviraShop",
  description: "فروشگاه تخصصی ماگ نویرا",
};

interface RootLayoutProps {
  children: React.ReactNode;
}

export default function RootLayout({ children }: RootLayoutProps) {
  return (
    <html lang="fa" dir="rtl" className={vazirmatn.variable}>
      <body>
        <AppProviders>
          <Header />
          {children}
          <Footer />
        </AppProviders>
      </body>
    </html>
  );
}
