/** @typedef {import("../core/types.js").GridRequest} GridRequest */
/** @typedef {import("../core/types.js").GridResponse<TData>} GridResponse @template TData */
/** @typedef {import("../core/types.js").GridQueryOptions} GridQueryOptions */

import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { useMemo } from "react";
import { fetchGrid } from "../api/grid-api.js";
import { gridQueryKey } from "../core/query-key.js";
import { normalizeRequest } from "../core/normalization.js";

/**
 * Server-side grid query with previous-data preservation so pagination
 * and filter changes never destroy the currently rendered rows.
 *
 * @template TData
 * @param {Object} args
 * @param {string} args.gridKey
 * @param {string} args.endpoint
 * @param {GridRequest} args.request
 * @param {GridQueryOptions} [args.queryOptions]
 */
export function useGridQuery({ gridKey, endpoint, request, queryOptions }) {
  const normalized = useMemo(() => normalizeRequest(request), [request]);
  const queryKey = useMemo(
    () => gridQueryKey(gridKey, endpoint, normalized),
    [gridKey, endpoint, normalized]
  );

  return useQuery({
    queryKey,
    queryFn: ({ signal }) => fetchGrid(endpoint, normalized, signal),
    placeholderData: keepPreviousData,
    staleTime: queryOptions?.staleTime ?? 30_000,
    gcTime: queryOptions?.gcTime,
    enabled: queryOptions?.enabled !== false,
    retry: (failureCount, error) => {
      // Never retry auth/authorization failures — centralized handling owns them.
      const status = /** @type {{status?: number}} */ (error)?.status;
      if (status === 401 || status === 403) return false;
      return failureCount < 2;
    },
  });
}
