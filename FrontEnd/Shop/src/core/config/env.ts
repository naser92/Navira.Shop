// Centralized runtime configuration. Never read process.env directly
// from feature or framework code; import from here instead.

function readEnv(key: string, fallback?: string): string {
  const value = process.env[key] ?? fallback;

  if (value === undefined) {
    throw new Error(`Missing required environment variable: ${key}`);
  }

  return value;
}

export const env = {
  apiBaseUrl: readEnv("NEXT_PUBLIC_API_BASE_URL", "http://localhost:5000/api"),
};
