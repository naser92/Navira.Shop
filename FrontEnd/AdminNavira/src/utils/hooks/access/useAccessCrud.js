"use client";

import { useQueryClient } from "@tanstack/react-query";
import { apiFetch } from "@/lib/api/clientApi";
import Toast from "@/lib/toast";
import useCustomQuery from "@/utils/hooks/useCustomQuery";
import useCustomMutation from "@/utils/hooks/useCustomMutation";

const normalizeList = (payload) => {
  if (Array.isArray(payload)) return payload;
  if (Array.isArray(payload?.items)) return payload.items;
  if (Array.isArray(payload?.data)) return payload.data;
  return [];
};

export function useAccessList(resource, params = {}) {
  const searchParams = new URLSearchParams();
  Object.entries(params).forEach(([key, value]) => {
    if (value !== undefined && value !== null && value !== "") {
      searchParams.set(key, value);
    }
  });
  const queryString = searchParams.toString();

  const query = useCustomQuery(
    ["access", resource, queryString],
    () => apiFetch(`/api/access/${resource}${queryString ? `?${queryString}` : ""}`),
    { refetchOnWindowFocus: false }
  );

  return { ...query, items: normalizeList(query.data?.data) };
}

export function useAccessAssign(resource) {
  const queryClient = useQueryClient();
  return useCustomMutation(
    ({ id, body }) =>
      apiFetch(`/api/access/${resource}/${id}`, {
        method: "PUT",
        body: JSON.stringify(body),
      }),
    {
      onSuccess: () => {
        Toast.success("با موفقیت ذخیره شد");
        queryClient.invalidateQueries({ queryKey: ["access"] });
      },
      onError: (error) => {
        Toast.error(error.message || "ذخیره تغییرات ناموفق بود");
      },
    }
  );
}

export function useAccessCreate(resource) {
  const queryClient = useQueryClient();
  return useCustomMutation(
    (body) =>
      apiFetch(`/api/access/${resource}`, {
        method: "POST",
        body: JSON.stringify(body),
      }),
    {
      onSuccess: () => {
        Toast.success("با موفقیت ایجاد شد");
        queryClient.invalidateQueries({ queryKey: ["access"] });
      },
      onError: (error) => {
        Toast.error(error.message || "ایجاد مورد ناموفق بود");
      },
    }
  );
}
