"use client";

import { useCallback, useEffect, useMemo, useState } from "react";
import { useRouter } from "next/navigation";
import AccountContext from "./accountContext";
import { apiFetch } from "@/lib/api/clientApi";

let authTokens = {
  accessToken: null,
  refreshToken: null,
};

export const getAuthTokens = () => authTokens;

export const setAuthTokens = ({ accessToken, refreshToken }) => {
  authTokens = {
    accessToken: accessToken ?? authTokens.accessToken,
    refreshToken: refreshToken ?? authTokens.refreshToken,
  };
};

export const clearAuthTokens = () => {
  authTokens = {
    accessToken: null,
    refreshToken: null,
  };
};

export default function AccountProvider({ children }) {
  const router = useRouter();

  const [userInfo, setUserInfo] = useState(null);
  const [userAccess, setUserAccess] = useState([]);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [tokens, setTokens] = useState({ accessToken: null, refreshToken: null });

  const storeTokens = useCallback((newTokens) => {
    setAuthTokens(newTokens || {});
    setTokens({
      accessToken: newTokens?.accessToken ?? null,
      refreshToken: newTokens?.refreshToken ?? null,
    });
  }, []);

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
      clearAuthTokens();
      setTokens({ accessToken: null, refreshToken: null });
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
      clearAuthTokens();
      setTokens({ accessToken: null, refreshToken: null });
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
      accessToken: tokens.accessToken,
      refreshToken: tokens.refreshToken,
      storeTokens,
      refreshProfile,
      logout,
    }),
    [userInfo, userAccess, isAuthenticated, isLoading, tokens, storeTokens, refreshProfile, logout]
  );

  return <AccountContext.Provider value={value}>{children}</AccountContext.Provider>;
}
