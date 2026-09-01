import { copyFileSync, existsSync } from "node:fs";
import { join } from "node:path";

const serverDirectory = join(process.cwd(), "dist", "server");
const moduleEntrypoint = join(serverDirectory, "index.mjs");
const sitesEntrypoint = join(serverDirectory, "index.js");

if (!existsSync(moduleEntrypoint)) {
  throw new Error("Vinext server entrypoint was not produced.");
}

// Sites currently discovers the runtime through dist/server/index.js.
// Vinext emits the same ESM bundle as index.mjs, so keep both filenames.
copyFileSync(moduleEntrypoint, sitesEntrypoint);
