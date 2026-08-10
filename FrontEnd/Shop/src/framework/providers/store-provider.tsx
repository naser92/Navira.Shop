"use client";
// Client component required: Redux Provider relies on React context and
// a store instance created per client (docs/ARCHITECTURE.md section 7).

import { useRef, type ReactNode } from "react";
import { Provider } from "react-redux";

import { makeStore, type AppStore } from "@/store";

interface StoreProviderProps {
  children: ReactNode;
}

export function StoreProvider({ children }: StoreProviderProps) {
  const storeRef = useRef<AppStore | null>(null);

  if (!storeRef.current) {
    storeRef.current = makeStore();
  }

  return <Provider store={storeRef.current}>{children}</Provider>;
}
