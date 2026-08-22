"use client";

import { useCallback, useEffect, useMemo, useState } from "react";
import { useRouter } from "next/navigation";
import AccountContext from "./accountContext";
import { apiFetch } from "@/lib/api/clientApi";

export default function AccountProvider({ children }) {
  const router = useRouter();

  const [userInfo, setUserInfo] = useState(null);
  const [userAccess, setUserAccess] = useState([]);
  const [dynamicMenus, setDynamicMenus] = useState(null);
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
      setDynamicMenus(null);
      setIsAuthenticated(false);
      return null;
    } finally {
      setIsLoading(false);
    }
  }, []);

  const refreshUserAccessInfo = useCallback(async () => {
    try {
      const result = await apiFetch("/api/auth/UserAccessInfo", {
        method: "GET",
      });

      const menuData = result?.data?.menus || [];
      setDynamicMenus(menuData);

      return menuData;
    } catch (error) {
      console.error("Failed to fetch user access info:", error);
      setDynamicMenus(null);
      return null;
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
      setDynamicMenus(null);
      setIsAuthenticated(false);
      router.replace("/auth/login");
      router.refresh();
    }
  }, [router]);

  useEffect(() => {
    refreshProfile();
    
    // Listen for logout events from the API client
    const handleLogoutEvent = () => {
      setUserInfo(null);
      setUserAccess([]);
      setDynamicMenus(null);
      setIsAuthenticated(false);
    };
    
    window.addEventListener("auth:logout", handleLogoutEvent);
    
    return () => {
      window.removeEventListener("auth:logout", handleLogoutEvent);
    };
  }, [refreshProfile]);

  const value = useMemo(
    () => ({
      userInfo,
      userAccess,
      dynamicMenus,
      refreshUserAccessInfo,
      isAuthenticated,
      isLoading,
      refreshProfile,
      logout,
    }),
    [userInfo, userAccess, isAuthenticated, isLoading, refreshProfile, logout, dynamicMenus, refreshUserAccessInfo]
  );

  return <AccountContext.Provider value={value}>{children}</AccountContext.Provider>;
}
