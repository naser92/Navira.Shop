"use client";

import AccountContext from "@/helper/accountContext/accountContext";
import SettingContext from "@/helper/settingContext";
import Image from "next/image";
import Link from "next/link";
import { useContext, useEffect, useRef, useState } from "react";
import { RiArrowDownSLine, RiLogoutBoxRLine, RiMenuLine, RiUser3Line } from "react-icons/ri";

const Header = () => {
  const { state, sidebarOpen, setSidebarOpen } = useContext(SettingContext);
  const { userInfo, logout } = useContext(AccountContext);
  const [profileOpen, setProfileOpen] = useState(false);
  const profileRef = useRef(null);

  const logoSrc = state?.setDarkLogo?.original_url || "/assets/images/logo.png";
  const displayName = userInfo?.fullName || userInfo?.userName || "مدیر";
  const avatarSrc = userInfo?.avatar || userInfo?.profileImage || userInfo?.profile_image?.original_url || null;

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (profileRef.current && !profileRef.current.contains(event.target)) {
        setProfileOpen(false);
      }
    };
    const handleEscape = (event) => {
      if (event.key === "Escape") {
        setProfileOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    document.addEventListener("keydown", handleEscape);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
      document.removeEventListener("keydown", handleEscape);
    };
  }, []);

  return (
    <div className={`page-header navira-header ${sidebarOpen ? "close_icon" : ""}`} dir="rtl">
      <div className="header-wrapper navira-header-wrapper m-0">
        <div className="header-logo-wrapper navira-header-brand p-0 d-flex align-items-center gap-2">
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

        <div className="nav-right navira-header-actions d-flex align-items-center" ref={profileRef}>
          <button
            type="button"
            className={`navira-profile-btn d-flex align-items-center ${profileOpen ? "open" : ""}`}
            onClick={() => setProfileOpen(!profileOpen)}
            aria-haspopup="menu"
            aria-expanded={profileOpen}
            aria-label="منوی کاربر"
          >
            <span className="navira-avatar">
              {avatarSrc ? (
                <Image src={avatarSrc} alt={displayName} width={34} height={34} className="navira-avatar-img" />
              ) : (
                <RiUser3Line size={17} />
              )}
            </span>
            <RiArrowDownSLine size={18} className="navira-profile-caret" />
          </button>
          {profileOpen && (
            <div className="navira-profile-menu" role="menu">
              <div className="navira-profile-menu-header">
                <span className="navira-avatar navira-avatar-lg">
                  {avatarSrc ? (
                    <Image src={avatarSrc} alt={displayName} width={42} height={42} className="navira-avatar-img" />
                  ) : (
                    <RiUser3Line size={20} />
                  )}
                </span>
                <div className="navira-profile-menu-info">
                  <span className="navira-profile-menu-name">{displayName}</span>
                  {userInfo?.userName && <span className="navira-profile-menu-username">@{userInfo.userName}</span>}
                </div>
              </div>
              <ul className="navira-profile-menu-list">
                <li role="none">
                  <Link href="/profile" role="menuitem" onClick={() => setProfileOpen(false)}>
                    <RiUser3Line size={17} />
                    <span>پروفایل</span>
                  </Link>
                </li>
                <li role="none">
                  <button type="button" role="menuitem" className="navira-menu-logout" onClick={logout}>
                    <RiLogoutBoxRLine size={17} />
                    <span>خروج از حساب</span>
                  </button>
                </li>
              </ul>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Header;
