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

export async function apiFetch(url, options = {}) {
  const response = await fetch(url, {
    ...options,
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
      ...(options.headers || {}),
    },
  });

  // Handle 401 Unauthorized - try to refresh token
  if (response.status === 401) {
    // Don't refresh for auth endpoints (login, refresh, etc.)
    if (url.includes("/api/auth/")) {
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

  // Parse the response
  const data = await parseResponse(response);

  // Treat HTTP errors AND business-logic errors (error: true / success: false)
  // as failures so callers always land in their catch block with the backend message.
  const isError = !response.ok || data?.error === true || data?.success === false;

  if (isError) {
    const error = new Error(data?.message || "Request failed");
    error.status = response.status;
    error.data = data;
    throw error;
  }

  return data;
}

async function handle401Response(originalUrl, originalOptions) {
  // If already refreshing, wait for the refresh to complete
  if (isRefreshing) {
    return new Promise((resolve, reject) => {
      subscribeToRefresh((error, token) => {
        if (error) {
          reject(error);
        } else {
          // Retry the original request with the new token
          retryOriginalRequest(originalUrl, originalOptions)
            .then(resolve)
            .catch(reject);
        }
      });
    });
  }

  // Start refresh process
  isRefreshing = true;
  const refreshResult = await refreshTokenRequest();

  try {
    if (!refreshResult.success) {
      throw new Error(refreshResult.message || "Refresh failed");
    }
    
    // Retry the original request
    const result = await retryOriginalRequest(originalUrl, originalOptions);
    return result;
  } catch (refreshError) {
    // Refresh failed, clear auth and redirect to login
    clearAuthAndRedirect();
    throw refreshError;
  } finally {
    isRefreshing = false;
    notifyRefreshSubscribers();
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

  if (!response.ok) {
    const errorData = await parseResponse(response);
    const error = new Error(errorData?.message || "Request failed");
    error.status = response.status;
    error.data = errorData;
    throw error;
  }

  return parseResponse(response);
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
  // Clear any stored auth data
  // This will trigger the auth context to update
  const event = new CustomEvent("auth:logout");
  window.dispatchEvent(event);
  
  // Redirect to login using location
  window.location.href = "/auth/login";
}

function redirectTo403() {
  // Prevent redirect loops
  if (window.location.pathname !== "/403") {
    window.location.href = "/403";
  }
}
