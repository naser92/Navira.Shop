"use client";

import { useState } from "react";
import { Provider } from "react-redux";
import { makeStore } from "./index.js";

/**
 * Client-side Redux provider. The store is created once per client
 * component tree via a lazy useState initializer, per App Router guidance.
 */
export default function StoreProvider({ children }) {
  const [store] = useState(() => makeStore());
  return <Provider store={store}>{children}</Provider>;
}
