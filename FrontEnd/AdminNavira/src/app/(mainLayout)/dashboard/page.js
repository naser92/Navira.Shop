"use client";

import AccountContext from "@/helper/accountContext/accountContext";
import { useContext } from "react";
import { Col, Row } from "reactstrap";

const dashboardTexts = {
  Welcome: "خوش آمدید",
  AdminPanel: "به پنل مدیریت نویرا خوش آمدید",
  DefaultUser: "مدیر",
  DashboardDescription: "از منوی سمت راست می‌توانید به بخش‌های مختلف پنل دسترسی داشته باشید.",
};

const DashboardPage = () => {
  const { userInfo } = useContext(AccountContext);
  const displayName = userInfo?.fullName || userInfo?.userName || dashboardTexts.DefaultUser;

  return (
    <Row dir="rtl">
      <Col sm="12">
        <div className="dashboard-welcome-box">
          <h1>
            {dashboardTexts.Welcome}، {displayName}
          </h1>
          <h4>{dashboardTexts.AdminPanel}</h4>
          <p>{dashboardTexts.DashboardDescription}</p>
        </div>
      </Col>
    </Row>
  );
};

export default DashboardPage;
