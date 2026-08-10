"use client";
// Client component required: instantiates a stateful QueryClient and
// TanStack Query's DevTools/hooks context (docs/ARCHITECTURE.md section 7).

import { useState, type ReactNode } from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

import { createQueryClient } from "@/core/query/query-client";

interface QueryProviderProps {
  children: ReactNode;
}

export function QueryProvider({ children }: QueryProviderProps) {
  const [queryClient] = useState<QueryClient>(() => createQueryClient());

  return <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>;
}
