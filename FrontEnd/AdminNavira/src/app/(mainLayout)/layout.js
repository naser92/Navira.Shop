"use client";

import { useContext, useEffect } from "react";
import { useRouter } from "next/navigation";
import Layout from "@/layout";
import AccountContext from "@/helper/accountContext/accountContext";

function ProtectedContent({ children }) {
  const router = useRouter();
  const {
    isAuthenticated,
    isLoading,
    userAccessReady,
    dynamicMenuLoading,
    dynamicMenuError,
  } = useContext(AccountContext);

  useEffect(() => {
    if (!isLoading && !isAuthenticated) {
      router.replace("/auth/login");
    }
  }, [isAuthenticated, isLoading, router]);

  if (isLoading) {
    return <div className="p-4">Loading...</div>;
  }

  if (!isAuthenticated) {
    return null;
  }

  if ((dynamicMenuLoading || !userAccessReady) && !dynamicMenuError) {
    return <div className="p-4">در حال آماده‌سازی پنل...</div>;
  }

  return <Layout>{children}</Layout>;
}

export default function RootLayout({ children }) {
  return <ProtectedContent>{children}</ProtectedContent>;
}
