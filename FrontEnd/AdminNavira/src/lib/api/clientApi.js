// Shared refresh lock to prevent multiple refresh requests
let isRefreshing = false;
let refreshSubscribers = [];

const subscribeToRefresh = (callback) => {
  refreshSubscribers.push(callback);
};

const notifyRefreshSubscribers = (error = null, token = null) => {
  refreshSubscribers.forEach((callback) => callback(error, token));
  refreshSubscribers = [];
};

const isAuthErrorEndpoint = (url) =>
  Boolean(
    url?.includes("/api/auth/login") ||
      url?.includes("/api/auth/refresh") ||
      url?.includes("/api/auth/logout") ||
      url?.includes("/api/auth/register") ||
      url?.includes("/api/auth/forgot") ||
      url?.includes("/api/auth/me") ||
      url?.includes("/api/auth/UserAccessInfo")
  );

export async function apiFetch(url, options = {}) {
  const response = await fetch(url, {
    ...options,
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
      ...(options.headers || {}),
    },
  });

  if (response.status === 401) {
    if (isAuthErrorEndpoint(url)) {
      const errorData = await parseResponse(response);
      const error = new Error(errorData?.message || "Authentication failed");
      error.status = response.status;
      error.data = errorData;
      throw error;
    }

    return handle401Response(url, options);
  }

  // Handle 403 Forbidden - redirect to 403 page
  if (response.status === 403) {
    redirectTo403();
    const errorData = await parseResponse(response);
    const error = new Error(errorData?.message || "Access forbidden");
    error.status = response.status;
    error.data = errorData;
    throw error;
  }

  const data = await parseResponse(response);

  const isError = !response.ok || data?.error === true || data?.success === false;

  if (isError) {
    const error = new Error(data?.message || "Request failed");
    error.status = response.status;
    error.error = data?.error;
    error.data = data;
    throw error;
  }

  return data;
}

async function handle401Response(originalUrl, originalOptions) {
  if (isRefreshing) {
    return new Promise((resolve, reject) => {
      subscribeToRefresh((error, token) => {
        if (error) {
          reject(error);
        } else {
          retryOriginalRequest(originalUrl, originalOptions)
            .then(resolve)
            .catch(reject);
        }
      });
    });
  }

  isRefreshing = true;

  try {
    const refreshResult = await refreshTokenRequest();

    if (!refreshResult.success) {
      throw new Error(refreshResult.message || "Refresh failed");
    }

    const result = await retryOriginalRequest(originalUrl, originalOptions);
    notifyRefreshSubscribers(null, refreshResult?.data?.accessToken || null);
    return result;
  } catch (refreshError) {
    notifyRefreshSubscribers(refreshError);
    clearAuthAndRedirect();
    throw refreshError;
  } finally {
    isRefreshing = false;
  }
}

async function retryOriginalRequest(url, options) {
  const response = await fetch(url, {
    ...options,
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
      ...(options.headers || {}),
    },
  });

  const data = await parseResponse(response);

  if (!response.ok) {
    const error = new Error(data?.message || "Request failed");
    error.status = response.status;
    error.data = data;
    throw error;
  }

  return data;
}

async function refreshTokenRequest() {
  try {
    const response = await fetch("/api/auth/refresh", {
      method: "POST",
      credentials: "include",
    });

    if (!response.ok) {
      const errorData = await parseResponse(response);
      throw new Error(errorData?.message || "Refresh failed");
    }

    const result = await response.json();
    return result;
  } catch (error) {
    console.error("Token refresh failed:", error);
    return { success: false, message: error.message };
  }
}

async function parseResponse(response) {
  let data = null;
  try {
    data = await response.json();
  } catch {
    data = null;
  }
  return data;
}

function clearAuthAndRedirect() {
  const event = new CustomEvent("auth:logout");
  window.dispatchEvent(event);
  if (window.location.pathname !== "/auth/login") {
    window.location.href = "/auth/login";
  }
}

function redirectTo403() {
  if (window.location.pathname !== "/403") {
    window.location.href = "/403";
  }
}
