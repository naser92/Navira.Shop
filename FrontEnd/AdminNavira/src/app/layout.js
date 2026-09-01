"use client";

import { useState } from "react";
import AccountProvider from "@/helper/accountContext/AccountProvider";
import SettingProvider from "@/helper/settingContext/SettingProvider";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import ToastProvider from "@/lib/toast/ToastProvider";
import StoreProvider from "@/store/StoreProvider";
import "../../public/assets/scss/app.scss";

export default function RootLayout({ children }) {
  const [queryClient] = useState(() => new QueryClient());

  return (
    <html dir="rtl" lang="fa" data-scroll-behavior="smooth" suppressHydrationWarning={true}>
      <head>
        <meta name="darkreader-lock" />
        <link rel="preconnect" href="https://fonts.googleapis.com" />
        <link
          rel="preconnect"
          href="https://fonts.gstatic.com"
          crossOrigin="true"
        />
        <link
          href="https://fonts.googleapis.com/css2?family=Montserrat:ital,wght@0,100..900;1,100..900&display=swap"
          rel="stylesheet"
        ></link>
      </head>
      <body>
        <StoreProvider>
          <QueryClientProvider client={queryClient}>
          <SettingProvider>
            <AccountProvider>{children}</AccountProvider>
            <ToastProvider />
          </SettingProvider>
        </QueryClientProvider>
        </StoreProvider>
      </body>
    </html>
  );
}
