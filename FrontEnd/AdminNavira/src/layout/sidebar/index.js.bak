"use client";

import SettingContext from "@/helper/settingContext";
import Image from "next/image";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { useContext, useEffect, useState } from "react";
import { RiAddLine, RiCloseLine, RiSubtractLine } from "react-icons/ri";
import MENUITEMS from "./MenuData";

const SidebarMenuList = ({ menu, level, onNavigate }) => {
  const pathname = usePathname();
  const [openMenus, setOpenMenus] = useState([]);

  useEffect(() => {
    const active = [];
    menu?.forEach((item) => {
      if (item.children?.some((child) => child.path && pathname.startsWith(child.path))) {
        active.push(item.title);
      }
    });
    if (active.length) {
      setOpenMenus((prev) => [...new Set([...prev, ...active])]);
    }
  }, [pathname]);

  const toggleMenu = (title) => {
    setOpenMenus((prev) =>
      prev.includes(title) ? prev.filter((item) => item !== title) : [...prev, title]
    );
  };

  return (
    <>
      {menu?.map((item, index) => {
        const isOpen = openMenus.includes(item.title);
        const isActive = item.path && pathname.startsWith(item.path);

        return (
          <li className={`sidebar-list ${isOpen ? "active" : ""}`} key={`${level}-${index}`}>
            {item.path ? (
              <Link
                href={item.path}
                onClick={onNavigate}
                className={`sidebar-link sidebar-title link-nav ${isActive ? "active" : ""}`}
              >
                <div className="svg-icon">{item.icon}</div>
                <span>{item.title}</span>
              </Link>
            ) : (
              <>
                <a
                  className={`sidebar-link sidebar-title ${isOpen ? "active" : ""}`}
                  onClick={() => toggleMenu(item.title)}
                >
                  <div className="svg-icon">{item.icon}</div>
                  <span>{item.title}</span>
                  {item.children && (
                    <div className="according-menu">
                      {isOpen ? <RiSubtractLine /> : <RiAddLine />}
                    </div>
                  )}
                </a>
                {item.children && (
                  <ul className={`sidebar-submenu ${isOpen ? "menu-open" : ""}`}>
                    <SidebarMenuList menu={item.children} level={level + 1} onNavigate={onNavigate} />
                  </ul>
                )}
              </>
            )}
          </li>
        );
      })}
    </>
  );
};

const Sidebar = () => {
  const { state, sidebarOpen, setSidebarOpen } = useContext(SettingContext);
  const logoSrc = state?.setDarkLogo?.original_url || "/assets/images/logo.png";
  const closeSidebar = () => setSidebarOpen(false);

  return (
    <div className={`sidebar-wrapper ${sidebarOpen ? "sidebar-open" : ""}`} dir="rtl">
      <div className="logo-wrapper d-flex align-items-center justify-content-between">
        <Link href="/dashboard" onClick={closeSidebar}>
          <Image src={logoSrc} alt="Navira" width={140} height={28} priority />
        </Link>
        <button
          type="button"
          className="btn btn-link p-1 d-lg-none sidebar-close-btn"
          onClick={closeSidebar}
          aria-label="بستن منو"
        >
          <RiCloseLine size={22} />
        </button>
      </div>
      <nav className="sidebar-main">
        <div id="sidebar-menu">
          <ul className="sidebar-links" id="simple-bar">
            <SidebarMenuList menu={MENUITEMS} level={0} onNavigate={closeSidebar} />
          </ul>
        </div>
      </nav>
    </div>
  );
};

export default Sidebar;
