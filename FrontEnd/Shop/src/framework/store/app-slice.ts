import { createSlice } from "@reduxjs/toolkit";

// Global, cross-feature UI state (docs/ARCHITECTURE.md section 10.3).
// Kept in `framework` because it is not owned by any single feature.
// Serves as the initial reducer key so the root store is valid before
// any feature slice is registered.
interface AppState {
  isBootstrapped: boolean;
}

const initialState: AppState = {
  isBootstrapped: true,
};

const appSlice = createSlice({
  name: "app",
  initialState,
  reducers: {},
});

export default appSlice.reducer;
