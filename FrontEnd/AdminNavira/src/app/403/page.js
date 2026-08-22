"use client";

import { Card, CardBody, CardHeader, Container, Row, Col } from "reactstrap";
import { useRouter } from "next/navigation";
import { RiLockUnlockLine, RiArrowLeftLine } from "react-icons/ri";
import { useState, useEffect } from "react";

const ForbiddenPage = () => {
  const router = useRouter();
  const [timeLeft, setTimeLeft] = useState(10);

  const handleGoBack = () => {
    router.back();
  };

  const handleGoHome = () => {
    router.push('/');
  };

  // Countdown timer for auto-redirect
  useEffect(() => {
    const timer = setInterval(() => {
      setTimeLeft(prev => {
        if (prev <= 1) {
          clearInterval(timer);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  useEffect(() => {
    if (timeLeft <= 0) {
      handleGoHome();
    }
  }, [timeLeft]);

  return (
    <Container fluid className="p-4">
      <Row className="justify-content-center">
        <Col md="8" lg="6">
          <Card className="text-center border-0 shadow">
            <CardHeader className="bg-transparent border-0 pb-0">
              <div className="d-flex justify-content-center mb-4">
                <div className="bg-light rounded-circle d-flex align-items-center justify-content-center" style={{ width: 80, height: 80 }}>
                  <RiLockUnlockLine size={40} className="text-warning" />
                </div>
              </div>
            </CardHeader>
            <CardBody>
              <h1 className="display-1 fw-bold text-warning">403</h1>
              <h3 className="mb-3">دسترسی ممنوع</h3>
              <p className="lead mb-4">
                متاسفانه شما مجوز لازم برای دسترسی به این صفحه را ندارید.
              </p>
              <div className="alert alert-info text-start" dir="rtl">
                <small className="text-muted">
                  شما در عرض {timeLeft} ثانیه به صفحه اصلی منتقل خواهید شد.
                </small>
              </div>
              <div className="d-grid gap-2 d-md-flex justify-content-md-center mt-4">
                <button
                  type="button"
                  className="btn btn-outline-secondary me-md-2"
                  onClick={handleGoBack}
                >
                  <RiArrowLeftLine className="me-1" />
                  بازگشت
                </button>
                <button
                  type="button"
                  className="btn btn-primary"
                  onClick={handleGoHome}
                >
                  صفحه اصلی
                </button>
              </div>
            </CardBody>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default ForbiddenPage;
