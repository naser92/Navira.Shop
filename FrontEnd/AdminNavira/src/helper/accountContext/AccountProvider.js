"use client";

import { useCallback, useEffect, useMemo, useState } from "react";
import { useRouter } from "next/navigation";
import AccountContext from "./accountContext";
import { apiFetch } from "@/lib/api/clientApi";

export default function AccountProvider({ children }) {
  const router = useRouter();

  const [userInfo, setUserInfo] = useState(null);
  const [userAccess, setUserAccess] = useState([]);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  const refreshProfile = useCallback(async () => {
    try {
      setIsLoading(true);

      const result = await apiFetch("/api/auth/me", {
        method: "GET",
      });

      const profile = result?.data || {};

      setUserInfo(profile?.userInfo || profile?.user || profile || null);
      setUserAccess(profile?.userAccess || profile?.permissions || []);
      setIsAuthenticated(true);

      return profile;
    } catch (error) {
      setUserInfo(null);
      setUserAccess([]);
      setIsAuthenticated(false);
      return null;
    } finally {
      setIsLoading(false);
    }
  }, []);

  const logout = useCallback(async () => {
    try {
      await apiFetch("/api/auth/logout", {
        method: "POST",
      });
    } finally {
      setUserInfo(null);
      setUserAccess([]);
      setIsAuthenticated(false);
      router.replace("/auth/login");
      router.refresh();
    }
  }, [router]);

  useEffect(() => {
    refreshProfile();
  }, [refreshProfile]);

  const value = useMemo(
    () => ({
      userInfo,
      userAccess,
      isAuthenticated,
      isLoading,
      refreshProfile,
      logout,
    }),
    [userInfo, userAccess, isAuthenticated, isLoading, refreshProfile, logout]
  );

  return <AccountContext.Provider value={value}>{children}</AccountContext.Provider>;
}
