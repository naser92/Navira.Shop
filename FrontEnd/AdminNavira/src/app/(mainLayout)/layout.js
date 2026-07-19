"use client";

import { useContext, useEffect } from "react";
import { useRouter } from "next/navigation";
import Layout from "@/layout";
import AccountProvider from "@/helper/accountContext/AccountProvider";
import AccountContext from "@/helper/accountContext/accountContext";


function ProtectedContent({ children }) {
  const router = useRouter();
  const { isAuthenticated, isLoading } = useContext(AccountContext);

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

  return <Layout>{children}</Layout>;
}

export default function RootLayout({ children }) {
  return (
    <AccountProvider>
      <ProtectedContent>{children}</ProtectedContent>
    </AccountProvider>
  );
}
