"use client";

import { useContext, useEffect } from "react";
import { Container } from "reactstrap";
import SettingContext from "@/helper/settingContext";
import Sidebar from "./sidebar";
import Header from "./header";
import Footer from "./footer";

const Layout = (props) => {
  const { sidebarOpen, setSidebarOpen } = useContext(SettingContext);

  // Start with the sidebar closed on mobile, open on desktop.
  useEffect(() => {
    const media = window.matchMedia("(max-width: 991px)");
    setSidebarOpen(!media.matches);

    const onChange = (e) => setSidebarOpen(!e.matches);
    media.addEventListener("change", onChange);
    return () => media.removeEventListener("change", onChange);
  }, [setSidebarOpen]);

  return (
    <div className="page-wrapper compact-wrapper" id="pageWrapper" dir="rtl">
      <Header />
      <div className="page-body-wrapper">
        <Sidebar />
        <div
          className={`bg-overlay1 ${sidebarOpen ? "active" : ""}`}
          onClick={() => setSidebarOpen(false)}
        />
        <div className="page-body">
          <Container fluid={true}>{props.children}</Container>
        </div>
        <Footer />
      </div>
    </div>
  );
};

export default Layout;
