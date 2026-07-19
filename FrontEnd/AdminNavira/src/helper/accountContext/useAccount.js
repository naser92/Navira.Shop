"use client";

import { useContext } from "react";
import AccountContext from "./accountContext";

export default function useAccount() {
  const context = useContext(AccountContext);

  if (!context) {
    throw new Error("useAccount must be used within AccountProvider");
  }

  return context;
}
