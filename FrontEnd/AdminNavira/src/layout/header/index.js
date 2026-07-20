"use client";

import AccountContext from "@/helper/accountContext/accountContext";
import SettingContext from "@/helper/settingContext";
import Image from "next/image";
import Link from "next/link";
import { useContext, useEffect, useRef, useState } from "react";
import { RiLogoutBoxRLine, RiMenuLine, RiUser3Line } from "react-icons/ri";

const Header = () => {
  const { state, sidebarOpen, setSidebarOpen } = useContext(SettingContext);
  const { userInfo, logout } = useContext(AccountContext);
  const [profileOpen, setProfileOpen] = useState(false);
  const profileRef = useRef(null);

  const logoSrc = state?.setDarkLogo?.original_url || "/assets/images/logo.png";
  const displayName = userInfo?.fullName || userInfo?.userName || "مدیر";

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (profileRef.current && !profileRef.current.contains(event.target)) {
        setProfileOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  return (
    <div className="page-header" dir="rtl">
      <div className="header-wrapper m-0">
        <div className="header-logo-wrapper p-0 d-flex align-items-center gap-2">
          <div className="logo-wrapper">
            <Link href="/dashboard">
              <Image src={logoSrc} alt="Navira" width={140} height={28} priority />
            </Link>
          </div>
          <button
            type="button"
            className="btn btn-link p-1 d-lg-none sidebar-toggle-btn"
            onClick={() => setSidebarOpen(!sidebarOpen)}
            aria-label="تغییر وضعیت منو"
          >
            <RiMenuLine size={22} />
          </button>
        </div>

        <div className="nav-right d-flex align-items-center" ref={profileRef}>
          <button
            type="button"
            className="btn btn-link d-flex align-items-center gap-2 profile-btn"
            onClick={() => setProfileOpen(!profileOpen)}
          >
            <RiUser3Line size={20} />
            <span>{displayName}</span>
          </button>
          {profileOpen && (
            <ul className="profile-dropdown">
              <li>
                <Link href="/profile" onClick={() => setProfileOpen(false)}>
                  <RiUser3Line /> پروفایل
                </Link>
              </li>
              <li>
                <a onClick={logout} style={{ cursor: "pointer" }}>
                  <RiLogoutBoxRLine /> خروج
                </a>
              </li>
            </ul>
          )}
        </div>
      </div>
    </div>
  );
};

export default Header;
