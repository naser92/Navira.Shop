"use client";

import { BiCheckShield, BiError } from "react-icons/bi";

const ShowBox = ({ showBoxMessage }) => {
  if (!showBoxMessage) return null;

  // Support both a plain string and a { message, type } object.
  const message =
    typeof showBoxMessage === "string" ? showBoxMessage : showBoxMessage?.message;
  const isSuccess =
    typeof showBoxMessage === "object" && showBoxMessage?.type === "success";

  if (!message) return null;

  return (
    <div className={isSuccess ? "success-box" : "error-box"} dir="rtl">
      {isSuccess ? <BiCheckShield /> : <BiError />}
      <div>
        <h4>{isSuccess ? "موفقیت" : "خطا"}</h4>
        <p>{message}</p>
      </div>
    </div>
  );
};

export default ShowBox;
