"use client";

import { useContext, useEffect } from "react";
import { useRouter } from "next/navigation";
import { Col, Container, Row } from "reactstrap";
import AccountContext from "@/helper/accountContext/accountContext";

const AuthLayout = ({ children }) => {
  const router = useRouter();
  const { isAuthenticated, isLoading } = useContext(AccountContext);

  useEffect(() => {
    if (!isLoading && isAuthenticated) {
      router.replace("/dashboard");
    }
  }, [isAuthenticated, isLoading, router]);

  return (
    <section className="log-in-section section-b-space">
      <Container className="w-100">
        <Row>
          <Col xl="5" lg="6" className="mx-auto">
            {children}
          </Col>
        </Row>
      </Container>
    </section>
  );
};

export default AuthLayout;
