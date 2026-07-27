"use client";

import AccountContext from "@/helper/accountContext/accountContext";
import { useContext } from "react";
import { Card, CardBody, Col, Row } from "reactstrap";
import { RiUser3Line } from "react-icons/ri";

const profileTexts = {
  Title: "پروفایل کاربری",
  Name: "نام",
  UserName: "نام کاربری",
  DefaultUser: "مدیر",
  EmptyState: "اطلاعات حساب کاربری پس از اتصال به بک‌اند در این بخش نمایش داده می‌شود.",
};

const ProfilePage = () => {
  const { userInfo } = useContext(AccountContext);
  const displayName = userInfo?.fullName || userInfo?.userName || profileTexts.DefaultUser;

  return (
    <Row dir="rtl">
      <Col sm="12" md="8" lg="6">
        <Card>
          <CardBody>
            <div className="d-flex align-items-center gap-3 mb-4">
              <span className="navira-avatar navira-avatar-lg">
                <RiUser3Line size={20} />
              </span>
              <h4 className="mb-0">{profileTexts.Title}</h4>
            </div>
            <ul className="list-unstyled mb-0">
              <li className="mb-2">
                <strong>{profileTexts.Name}: </strong>
                {displayName}
              </li>
              {userInfo?.userName && (
                <li className="mb-2">
                  <strong>{profileTexts.UserName}: </strong>
                  {userInfo.userName}
                </li>
              )}
            </ul>
            <p className="text-muted mt-4 mb-0">{profileTexts.EmptyState}</p>
          </CardBody>
        </Card>
      </Col>
    </Row>
  );
};

export default ProfilePage;
