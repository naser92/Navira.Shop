"use client";

import { useState } from "react";
import { Card, CardBody, Col, Nav, NavItem, NavLink, Row, TabContent, TabPane } from "reactstrap";
import { RiKey2Line, RiShieldUserLine, RiUserSettingsLine } from "react-icons/ri";
import PolicyList from "@/components/access/PolicyList";
import RoleList from "@/components/access/RoleList";
import PermissionList from "@/components/access/PermissionList";

const accessTexts = {
  Title: "مدیریت دسترسی‌ها",
  Description: "نقش‌ها را به سیاست‌ها و سیاست‌ها را به مجوزها متصل کنید.",
  Policies: "سیاست‌ها",
  Roles: "نقش‌ها",
  Permissions: "مجوزها",
};

const TABS = [
  { id: "roles", title: accessTexts.Roles, icon: <RiUserSettingsLine size={17} /> },
  { id: "policies", title: accessTexts.Policies, icon: <RiShieldUserLine size={17} /> },
  { id: "permissions", title: accessTexts.Permissions, icon: <RiKey2Line size={17} /> },
];

const AccessPage = () => {
  const [activeTab, setActiveTab] = useState("roles");

  return (
    <Row dir="rtl">
      <Col sm="12">
        <div className="navira-access-header">
          <h2>{accessTexts.Title}</h2>
          <p>{accessTexts.Description}</p>
        </div>

        <Card className="navira-access-card">
          <CardBody>
            <Nav pills className="navira-access-tabs">
              {TABS.map((tab) => (
                <NavItem key={tab.id}>
                  <NavLink
                    className={activeTab === tab.id ? "active" : ""}
                    onClick={() => setActiveTab(tab.id)}
                  >
                    {tab.icon}
                    <span>{tab.title}</span>
                  </NavLink>
                </NavItem>
              ))}
            </Nav>

            <TabContent activeTab={activeTab}>
              <TabPane tabId="roles">
                {activeTab === "roles" && <RoleList />}
              </TabPane>
              <TabPane tabId="policies">
                {activeTab === "policies" && <PolicyList />}
              </TabPane>
              <TabPane tabId="permissions">
                {activeTab === "permissions" && <PermissionList />}
              </TabPane>
            </TabContent>
          </CardBody>
        </Card>
      </Col>
    </Row>
  );
};

export default AccessPage;
