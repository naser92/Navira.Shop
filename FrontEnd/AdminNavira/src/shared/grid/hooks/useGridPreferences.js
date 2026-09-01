/** @typedef {import("../core/types.js").GridPreferences} GridPreferences */

import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import {
  clearPreferences,
  readPreferences,
  writePreferences,
} from "../core/preference-schema.js";
import { debounce } from "../core/utils.js";

const WRITE_DEBOUNCE_MS = 400;

/**
 * SSR-safe grid preferences with debounced Local Storage writes.
 *
 * @param {Object} args
 * @param {string} args.gridKey
 * @param {boolean} [args.enabled]
 */
export function useGridPreferences({ gridKey, enabled = true }) {
  // Lazy initializer reads Local Storage only on the client; the initial
  // server render (and first client render) always use null so hydration
  // stays consistent — preferences apply after mount via effect.
  const [preferences, setPreferences] = useState(
    /** @type {Partial<GridPreferences>|null} */ (null)
  );
  const [hydrated, setHydrated] = useState(false);

  useEffect(() => {
    if (!enabled) {
      setHydrated(true);
      return;
    }
    setPreferences(readPreferences(gridKey));
    setHydrated(true);
  }, [gridKey, enabled]);

  const persistRef = useRef(
    debounce((...args) => {
      const [key, prefs] = /** @type {[string, GridPreferences]} */ (args);
      writePreferences(key, prefs);
    }, WRITE_DEBOUNCE_MS)
  );

  useEffect(() => () => persistRef.current.cancel?.(), []);

  /** @type {(prefs: GridPreferences) => void} */
  const save = useCallback(
    (prefs) => {
      if (!enabled) return;
      setPreferences(prefs);
      persistRef.current(gridKey, prefs);
    },
    [enabled, gridKey]
  );

  const reset = useCallback(() => {
    persistRef.current.cancel?.();
    clearPreferences(gridKey);
    setPreferences(null);
  }, [gridKey]);

  return useMemo(
    () => ({ preferences, hydrated, save, reset }),
    [preferences, hydrated, save, reset]
  );
}
