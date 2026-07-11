"use client";
import { useEffect, useState } from "react";
import { Container } from "reactstrap";
import Footer from "./footer";

const Layout = (props) => {
    return(
         <div className="page-wrapper compact-wrapper" id="pageWrapper">
            <div className="page-body-wrapper">
                  <div className="page-body">
                        <Container fluid={true}>{props.children}</Container>
                           <Footer />
                  </div>
            </div> 
        </div>
    )
}



export default Layout