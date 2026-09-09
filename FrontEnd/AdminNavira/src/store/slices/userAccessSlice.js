import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { apiFetch } from "@/lib/api/clientApi";

const initialState = {
  userAccessInfo: null,
  menu: [],
  loading: false,
  error: null,
  loaded: false,
};

export const fetchUserAccessInfo = createAsyncThunk(
  "userAccess/fetchUserAccessInfo",
  async () => {
    const response = await apiFetch("/api/auth/UserAccessInfo", {
      method: "GET",
    });

    const payload =
      response?.data !== undefined && response?.data !== null ? response.data : response;

    const menus =
      payload?.menu ??
      payload?.result?.menu ??
      payload?.menus ??
      payload?.result?.menus ??
      payload?.data?.menu ??
      payload?.data?.menus ??
      [];

    return {
      userAccessInfo: payload || null,
      menu: Array.isArray(menus) ? menus : [],
    };
  }
);

const userAccessSlice = createSlice({
  name: "userAccess",
  initialState,
  reducers: {
    setUserAccessInfo(state, action) {
      state.userAccessInfo = action.payload;
      state.loaded = true;
      state.error = null;
    },
    setMenu(state, action) {
      state.menu = action.payload || [];
    },
    clearUserAccessInfo(state) {
      state.userAccessInfo = null;
      state.menu = [];
      state.loading = false;
      state.error = null;
      state.loaded = false;
    },
    setUserAccessLoading(state, action) {
      state.loading = action.payload;
    },
    setUserAccessLoaded(state, action) {
      state.loaded = Boolean(action.payload);
    },
    setUserAccessError(state, action) {
      state.error = action.payload || null;
      state.loading = false;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchUserAccessInfo.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchUserAccessInfo.fulfilled, (state, action) => {
        state.loading = false;
        state.loaded = true;
        state.userAccessInfo = action.payload.userAccessInfo || null;
        state.menu = action.payload.menu || [];
        state.error = null;
      })
      .addCase(fetchUserAccessInfo.rejected, (state, action) => {
        state.loading = false;
        state.loaded = false;
        state.error = action.error?.message || "خطا در دریافت منوی کاربر";
      });
  },
});

export const {
  setUserAccessInfo,
  setMenu,
  clearUserAccessInfo,
  setUserAccessLoading,
  setUserAccessLoaded,
  setUserAccessError,
} = userAccessSlice.actions;

export const selectUserAccessInfo = (state) => state.userAccess?.userAccessInfo || null;
export const selectMenu = (state) => state.userAccess?.menu || [];
export const selectUserAccessLoading = (state) => state.userAccess?.loading || false;
export const selectUserAccessLoaded = (state) => state.userAccess?.loaded || false;
export const selectUserAccessError = (state) => state.userAccess?.error || null;

export default userAccessSlice.reducer;
