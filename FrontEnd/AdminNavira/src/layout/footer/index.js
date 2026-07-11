import { useContext } from "react";
import { useTranslation } from "react-i18next";
import { Col, Container, Row } from "reactstrap";


const Footer = () => {
//   const { t } = useTranslation( 'common');
//   const { state } = useContext(SettingContext)
  return (
    <Container fluid={true}>
      <footer className="footer">
        <Row>
          <Col md="12" className="footer-copyright text-center">
          {/* <p className="mb-0">{t(state?.setCopyRight?state?.setCopyRight:'© Pixelstrap')}</p> */}
          <p className="mb-0">© Navira Shop</p>
          </Col>
        </Row>
      </footer>
    </Container>
  );
};

export default Footer;