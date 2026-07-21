"use client";

import { toast } from "react-toastify";

const defaultOptions = {
  position: "top-center",
  rtl: true,
  autoClose: 3500,
  hideProgressBar: false,
  closeOnClick: true,
  pauseOnHover: true,
  draggable: true,
};

const notify = (type, message, options = {}) =>
  toast(message, { ...defaultOptions, ...options, type });

const Toast = {
  success: (message, options) => notify("success", message, options),
  error: (message, options) => notify("error", message, options),
  warning: (message, options) => notify("warning", message, options),
  info: (message, options) => notify("info", message, options),
  // Convenience that maps common type aliases.
  show: (type, message, options) => {
    const map = { warn: "warning", err: "error" };
    return notify(map[type] || type || "info", message, options);
  },
  dismiss: (id) => (id ? toast.dismiss(id) : toast.dismiss()),
};

export default Toast;
