export async function apiFetch(url, options = {}) {
  const response = await fetch(url, {
    ...options,
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
      ...(options.headers || {}),
    },
  });

  let data = null;
  try {
    data = await response.json();
  } catch {
    data = null;
  }

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
