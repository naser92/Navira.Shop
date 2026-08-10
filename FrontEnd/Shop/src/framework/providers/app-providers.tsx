"use client";
// Composition root for all client-side providers. Kept as its own file so
// `app/layout.tsx` (a Server Component) can stay free of "use client".

import type { ReactNode } from "react";

import { QueryProvider } from "./query-provider";
import { StoreProvider } from "./store-provider";

interface AppProvidersProps {
  children: ReactNode;
}

export function AppProviders({ children }: AppProvidersProps) {
  return (
    <StoreProvider>
      <QueryProvider>{children}</QueryProvider>
    </StoreProvider>
  );
}
