/** @type {import('next').NextConfig} */
const path = require("path");

const nextConfig = {
  reactStrictMode: false,
  outputFileTracingRoot: path.join(__dirname),
  env: {
    // Replace below URL with your current Domain
    API_PROD_URL: "http://localhost:3000/api/",
    storageURL: "http://localhost:3000",
  },
  redirects: async () => {
    return [
      {
        source: "/",
        destination: "/dashboard",
        permanent: true,
      },
      {
        source: "/fa",
        destination: "/dashboard",
        permanent: true,
      },
    ];
  },
  images: {
    remotePatterns: [
      {
        protocol: "http",
        hostname: "localhost",
      },
      {
        protocol: "http",
        hostname: "127.0.0.1:3000",
      },
    ],
  },
};

module.exports = nextConfig;
