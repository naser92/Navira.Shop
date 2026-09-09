"use client";

import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { usePathname, useRouter } from "next/navigation";
import AccountContext from "./accountContext";
import { apiFetch } from "@/lib/api/clientApi";
import {
  clearAuth,
  setMenu,
  clearUserAccessInfo,
  setUserAccessError,
  setUserAccessInfo,
  setUserAccessLoaded,
  setUserAccessLoading,
  fetchUserAccessInfo,
  selectMenu,
  selectUserAccessError,
  selectUserAccessLoaded,
  selectUserAccessLoading,
} from "@/store";

const AUTH_ROUTE_PREFIX = "/auth";

const BootstrapState = {
  idle: "idle",
  checking: "checking",
  authenticated: "authenticated",
  unauthenticated: "unauthenticated",
};

const AccessState = {
  idle: "idle",
  loading: "loading",
  succeeded: "succeeded",
  failed: "failed",
};

const INITIAL_LOADING_STATE = {
  loading: false,
  loaded: false,
  menuLoading: false,
  menuLoaded: false,
};

const isAuthRoute = (pathname) =>
  typeof pathname === "string" && pathname.startsWith(AUTH_ROUTE_PREFIX);

export default function AccountProvider({ children }) {
  const pathname = usePathname();
  const router = useRouter();
  const dispatch = useDispatch();

  const inAuthRoute = isAuthRoute(pathname);

  const reduxMenu = useSelector(selectMenu);
  const reduxMenuLoading = useSelector(selectUserAccessLoading);
  const reduxMenuLoaded = useSelector(selectUserAccessLoaded);
  const reduxMenuError = useSelector(selectUserAccessError);

  const [userInfo, setUserInfo] = useState(null);
  const [userAccess, setUserAccess] = useState([]);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(INITIAL_LOADING_STATE.loading);
  const [authStatus, setAuthStatus] = useState(BootstrapState.idle);
  const [userAccessStatus, setUserAccessStatus] = useState(AccessState.idle);
  const [isSigningOut, setIsSigningOut] = useState(false);

  const meRequestRef = useRef(null);
  const userAccessRequestRef = useRef(null);
  const hasAuthSessionRef = useRef(false);
  const redirectingRef = useRef(false);

  const clearAuthState = useCallback(
    ({ toLogin = false, keepState = false } = {}) => {
      dispatch(clearAuth());
      dispatch(clearUserAccessInfo());
      dispatch(setUserAccessLoading(false));
      dispatch(setUserAccessError(null));
      dispatch(setUserAccessLoaded(false));
      dispatch(setUserAccessInfo(null));
      dispatch(setMenu([]));

      hasAuthSessionRef.current = false;
      setUserInfo(null);
      setUserAccess([]);
      setIsAuthenticated(false);
      setAuthStatus(BootstrapState.unauthenticated);
      setUserAccessStatus(AccessState.idle);
      setIsLoading(false);
      meRequestRef.current = null;
      userAccessRequestRef.current = null;

      if (toLogin && !inAuthRoute && !redirectingRef.current && !keepState) {
        redirectingRef.current = true;
        router.replace("/auth/login");
        setTimeout(() => {
          redirectingRef.current = false;
        }, 300);
      }
    },
    [dispatch, inAuthRoute, router]
  );

  const refreshProfile = useCallback(async ({ force = false } = {}) => {
    if (meRequestRef.current) {
      return meRequestRef.current;
    }

    if (isSigningOut) {
      return null;
    }

    if (inAuthRoute && !force) {
      return {
        userInfo,
        userAccess,
        isAuthenticated,
      };
    }

    if (
      !force &&
      (authStatus === BootstrapState.authenticated ||
        authStatus === BootstrapState.unauthenticated)
    ) {
      return {
        userInfo,
        userAccess,
        isAuthenticated,
      };
    }

    const request = (async () => {
      setIsLoading(true);
      setAuthStatus(BootstrapState.checking);

      try {
        const result = await apiFetch("/api/auth/me", {
          method: "GET",
        });

        const profile = result?.data || {};
        const nextUserInfo = profile?.userInfo || profile?.user || profile || null;
        const nextUserAccess = profile?.userAccess || profile?.permissions || [];

        setUserInfo(nextUserInfo);
        setUserAccess(nextUserAccess);
        setIsAuthenticated(true);
        hasAuthSessionRef.current = true;
        setAuthStatus(BootstrapState.authenticated);

        return {
          userInfo: nextUserInfo,
          userAccess: nextUserAccess,
        };
      } catch (error) {
        const isUnauthorized = error?.status === 401;
        clearAuthState({ toLogin: !inAuthRoute, keepState: false });
        setAuthStatus(isUnauthorized ? BootstrapState.unauthenticated : BootstrapState.idle);

        return null;
      } finally {
        setIsLoading(false);
      }
    })();

    meRequestRef.current = request;

    return request.finally(() => {
      meRequestRef.current = null;
    });
  }, [clearAuthState, inAuthRoute, authStatus, isAuthenticated, isSigningOut, userInfo, userAccess]);

  const refreshUserAccessInfo = useCallback(async () => {
    if (
      isSigningOut ||
      inAuthRoute ||
      !hasAuthSessionRef.current ||
      !isAuthenticated ||
      userAccessStatus === AccessState.loading
    ) {
      return null;
    }

    if (userAccessStatus === AccessState.succeeded || reduxMenuLoaded) {
      return {
        userAccessInfo: null,
        menu: reduxMenu || [],
      };
    }

    if (userAccessRequestRef.current) {
      return userAccessRequestRef.current;
    }

    const request = (async () => {
      setUserAccessStatus(AccessState.loading);
      dispatch(setUserAccessLoading(true));

      try {
        const result = await dispatch(fetchUserAccessInfo()).unwrap();
        setUserAccessStatus(AccessState.succeeded);
        dispatch(setUserAccessLoaded(true));
        return result;
      } catch (error) {
        const isUnauthorized = error?.status === 401;
        setUserAccessStatus(AccessState.failed);
        dispatch(setUserAccessError(error?.message || "خطا در دریافت منو"));

        if (isUnauthorized) {
          clearAuthState({ toLogin: true, keepState: false });
        }

        return {
          userAccessInfo: null,
          menu: [],
        };
      } finally {
        dispatch(setUserAccessLoading(false));
      }
    })();

    userAccessRequestRef.current = request;

    return request.finally(() => {
      userAccessRequestRef.current = null;
    });
  }, [
    clearAuthState,
    dispatch,
    inAuthRoute,
    isAuthenticated,
    isSigningOut,
    reduxMenu,
    reduxMenuLoaded,
    userAccessStatus,
  ]);

  const logout = useCallback(async () => {
    setIsSigningOut(true);

    try {
      await apiFetch("/api/auth/logout", {
        method: "POST",
      });
    } finally {
      setIsSigningOut(false);
      clearAuthState({ toLogin: true, keepState: false });
      router.refresh();
    }
  }, [clearAuthState, router]);

  useEffect(() => {
    if (!inAuthRoute) {
      refreshProfile();
    } else {
      setAuthStatus(BootstrapState.idle);
      setUserAccessStatus(AccessState.idle);
      setIsLoading(false);
    }

    const handleLogoutEvent = () => {
      setIsSigningOut(true);
      clearAuthState({ toLogin: true, keepState: false });
      setIsSigningOut(false);
    };

    window.addEventListener("auth:logout", handleLogoutEvent);

    return () => {
      window.removeEventListener("auth:logout", handleLogoutEvent);
    };
  }, [clearAuthState, inAuthRoute, refreshProfile]);

  useEffect(() => {
    if (
      !inAuthRoute &&
      !isSigningOut &&
      isAuthenticated &&
      hasAuthSessionRef.current &&
      userAccessStatus !== AccessState.loading &&
      !reduxMenuLoaded &&
      !reduxMenuLoading &&
      !reduxMenuError
    ) {
      refreshUserAccessInfo();
    }
  }, [
    inAuthRoute,
    isAuthenticated,
    isSigningOut,
    reduxMenuLoaded,
    reduxMenuLoading,
    reduxMenuError,
    refreshUserAccessInfo,
    userAccessStatus,
  ]);

  const value = useMemo(
    () => ({
      userInfo,
      userAccess,
      dynamicMenus: reduxMenu,
      dynamicMenuLoading: reduxMenuLoading,
      dynamicMenuError: reduxMenuError,
      refreshUserAccessInfo,
      isAuthenticated,
      isLoading,
      refreshProfile: ({ force } = {}) => refreshProfile({ force }),
      authSessionReady: hasAuthSessionRef.current,
      userAccessReady: reduxMenuLoaded,
      isSigningOut,
      initialState: INITIAL_LOADING_STATE,
      authStatus,
      userAccessStatus,
      bootstrapState: authStatus,
    }),
    [
      userInfo,
      userAccess,
      reduxMenu,
      reduxMenuLoading,
      reduxMenuError,
      refreshUserAccessInfo,
      isAuthenticated,
      isLoading,
      refreshProfile,
      isSigningOut,
      reduxMenuLoaded,
      authStatus,
      userAccessStatus,
    ]
  );

  return <AccountContext.Provider value={value}>{children}</AccountContext.Provider>;
}
