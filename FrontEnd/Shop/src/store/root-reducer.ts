import { combineReducers } from "@reduxjs/toolkit";

import appReducer from "@/framework/store/app-slice";

// Feature slices are registered here as they are introduced.
// Do not add feature-specific reducers directly to `core` or `framework`.
const rootReducer = combineReducers({
  app: appReducer,
});

export default rootReducer;
