import { createSlice } from "@reduxjs/toolkit";

/**
 * Auth slice.
 *
 * Tokens live in httpOnly cookies managed by the Next.js BFF layer
 * (see src/app/api/auth/* and src/lib/api/serverAuth.js). This slice holds
 * the client-visible mirror of that state so components can react to auth
 * changes without re-implementing token handling. Tokens are NOT persisted
 * to localStorage — the cookie-based BFF flow is the established mechanism.
 */

const initialState = {
  accessToken: null,
  refreshToken: null,
  user: null,
  isAuthenticated: false,
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setCredentials(state, action) {
      const { accessToken = null, refreshToken = null, user = null } =
        action.payload || {};
      state.accessToken = accessToken;
      state.refreshToken = refreshToken;
      if (user !== null) state.user = user;
      state.isAuthenticated = Boolean(accessToken);
    },
    setUser(state, action) {
      state.user = action.payload ?? null;
    },
    clearAuth(state) {
      state.accessToken = null;
      state.refreshToken = null;
      state.user = null;
      state.isAuthenticated = false;
    },
  },
});

export const { setCredentials, setUser, clearAuth } = authSlice.actions;

export const selectAuth = (state) => state.auth;
export const selectIsAuthenticated = (state) => state.auth.isAuthenticated;
export const selectUser = (state) => state.auth.user;

export default authSlice.reducer;
