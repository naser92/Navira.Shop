import { configureStore } from "@reduxjs/toolkit";
import authReducer from "./slices/authSlice.js";
import userAccessReducer from "./slices/userAccessSlice.js";

/**
 * Create a fresh store instance. In Next.js App Router the store must be
 * created per request/client boundary — never as a shared module-level
 * singleton — so StoreProvider calls this factory inside a useState
 * initializer.
 */
export function makeStore() {
  return configureStore({
    reducer: {
      auth: authReducer,
      userAccess: userAccessReducer,
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware(),
    devTools: process.env.NODE_ENV !== "production",
  });
}

export { setCredentials, setUser, clearAuth } from "./slices/authSlice.js";
export {
  fetchUserAccessInfo,
  setUserAccessInfo,
  setMenu,
  clearUserAccessInfo,
  setUserAccessLoading,
  setUserAccessLoaded,
  setUserAccessError,
  selectUserAccessInfo,
  selectMenu,
  selectUserAccessLoading,
  selectUserAccessLoaded,
  selectUserAccessError,
} from "./slices/userAccessSlice.js";
