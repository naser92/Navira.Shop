"use client";

import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const ToastProvider = () => (
  <ToastContainer
    position="top-center"
    rtl
    autoClose={3500}
    hideProgressBar={false}
    newestOnTop
    closeOnClick
    pauseOnFocusLoss
    draggable
    pauseOnHover
    theme="colored"
    limit={4}
  />
);

export default ToastProvider;
