"use client";

import { useEffect, type RefObject } from "react";

export function useHeroPointerEffect(rootRef: RefObject<HTMLElement | null>) {
  useEffect(() => {
    const root = rootRef.current;
    if (!root || window.matchMedia("(pointer: coarse), (prefers-reduced-motion: reduce)").matches) return;

    let frame = 0;
    const reset = () => {
      root.style.setProperty("--pointer-x", "0");
      root.style.setProperty("--pointer-y", "0");
      root.querySelectorAll<HTMLElement>("[data-model-letter]").forEach((letter) => {
        letter.style.setProperty("--scatter-x", "0px");
        letter.style.setProperty("--scatter-y", "0px");
      });
    };
    const move = (event: PointerEvent) => {
      cancelAnimationFrame(frame);
      frame = requestAnimationFrame(() => {
        const bounds = root.getBoundingClientRect();
        const x = event.clientX - bounds.left;
        const y = event.clientY - bounds.top;
        root.style.setProperty("--pointer-x", String((x / bounds.width - 0.5) * 2));
        root.style.setProperty("--pointer-y", String((y / bounds.height - 0.5) * 2));
        root.querySelectorAll<HTMLElement>("[data-model-letter]").forEach((letter) => {
          const box = letter.getBoundingClientRect();
          const dx = event.clientX - (box.left + box.width / 2);
          const dy = event.clientY - (box.top + box.height / 2);
          const distance = Math.hypot(dx, dy);
          const force = Math.max(0, 1 - distance / 190);
          letter.style.setProperty("--scatter-x", `${(-dx / Math.max(distance, 1)) * force * 22}px`);
          letter.style.setProperty("--scatter-y", `${(-dy / Math.max(distance, 1)) * force * 16}px`);
        });
      });
    };

    root.addEventListener("pointermove", move);
    root.addEventListener("pointerleave", reset);
    return () => {
      cancelAnimationFrame(frame);
      root.removeEventListener("pointermove", move);
      root.removeEventListener("pointerleave", reset);
    };
  }, [rootRef]);
}

