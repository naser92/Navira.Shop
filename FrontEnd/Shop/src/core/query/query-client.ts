import { QueryClient } from "@tanstack/react-query";

// Single QueryClient factory shared by the app. Kept in `core` per
// docs/ARCHITECTURE.md section 4.2 ("query client" is core infrastructure).
export function createQueryClient(): QueryClient {
  return new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: 60 * 1000,
        refetchOnWindowFocus: false,
        retry: 1,
      },
      mutations: {
        retry: 0,
      },
    },
  });
}
