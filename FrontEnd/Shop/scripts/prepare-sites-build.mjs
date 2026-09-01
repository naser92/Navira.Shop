import { existsSync, writeFileSync } from "node:fs";
import { join } from "node:path";

const serverDirectory = join(process.cwd(), "dist", "server");
const moduleEntrypoint = join(serverDirectory, "index.mjs");
const sitesEntrypoint = join(serverDirectory, "index.js");

if (!existsSync(moduleEntrypoint)) {
  throw new Error("Vinext server entrypoint was not produced.");
}

// Sites expects a Worker module whose default export exposes fetch().
// Vinext exports the request handler function directly from index.mjs.
writeFileSync(
  sitesEntrypoint,
  'import handler from "./index.mjs";\nexport default { fetch: handler };\n',
  "utf8",
);
