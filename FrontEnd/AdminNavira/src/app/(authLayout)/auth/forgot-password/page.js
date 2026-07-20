"use client";

import { ReactstrapInput } from "@/components/reactstrapFormik";
import ShowBox from "@/elements/alerts&Modals/ShowBox";
import Btn from "@/elements/buttons/Btn";
import SettingContext from "@/helper/settingContext";
import LoginBoxWrapper from "@/utils/hoc/LoginBoxWrapper";
import { YupObject, emailSchema } from "@/utils/validation/ValidationSchemas";
import { ErrorMessage, Field, Form, Formik } from "formik";
import Image from "next/image";
import Link from "next/link";
import { useContext, useState } from "react";
import { Col } from "reactstrap";
import { apiFetch } from "@/lib/api/clientApi";

const forgotPasswordTexts = {
  ForgotPasswordTitle: "بازیابی رمز عبور",
  ForgotPasswordDescription: "ایمیل خود را وارد کنید تا لینک بازیابی رمز عبور برای شما ارسال شود",
  Email: "ایمیل",
  SendResetLink: "ارسال لینک بازیابی",
  Sending: "...در حال ارسال",
  BackToLogin: "بازگشت به صفحه ورود",
  ResetLinkSent: "لینک بازیابی رمز عبور با موفقیت ارسال شد",
  SendFailed: "ارسال لینک بازیابی با خطا مواجه شد",
};

const ForgotPassword = () => {
  const [showBoxMessage, setShowBoxMessage] = useState();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const { state } = useContext(SettingContext);

  const handleForgotPassword = async (values, { resetForm }) => {
    try {
      setIsSubmitting(true);
      setShowBoxMessage(undefined);

      await apiFetch("/api/auth/forgot-password", {
        method: "POST",
        body: JSON.stringify({
          email: values.email,
        }),
      });

      setShowBoxMessage({
        message: forgotPasswordTexts.ResetLinkSent,
        type: "success",
      });
      resetForm();
    } catch (error) {
      setShowBoxMessage({
        message: error.message || forgotPasswordTexts.SendFailed,
        type: "danger",
      });
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="box-wrapper" dir="rtl">
      <ShowBox showBoxMessage={showBoxMessage} />
      <LoginBoxWrapper>
        <div className="log-in-title text-center">
          <Image
            className="for-white"
            src={state?.setDarkLogo?.original_url ? state?.setDarkLogo?.original_url : "/assets/images/logo.png"}
            alt="Light Logo"
            width={140}
            height={28}
            priority
          />
          <h3>{forgotPasswordTexts.ForgotPasswordTitle}</h3>
          <h4>{forgotPasswordTexts.ForgotPasswordDescription}</h4>
        </div>

        <div className="input-box">
          <Formik
            initialValues={{
              email: "",
            }}
            validationSchema={YupObject({
              email: emailSchema,
            })}
            onSubmit={handleForgotPassword}
          >
            {() => (
              <Form className="row g-4" dir="rtl">
                <Col sm="12">
                  <Field
                    inputprops={{ noExtraSpace: true }}
                    autoComplete={true}
                    name="email"
                    type="email"
                    component={ReactstrapInput}
                    className="form-control"
                    id="email"
                    placeholder={forgotPasswordTexts.Email}
                    label={forgotPasswordTexts.Email}
                  />
                </Col>

                <Col sm="12">
                  <Btn
                    title={isSubmitting ? forgotPasswordTexts.Sending : forgotPasswordTexts.SendResetLink}
                    className="btn btn-animation w-100 justify-content-center"
                    type="submit"
                    color="false"
                    disabled={isSubmitting}
                  />

                  <div className="sign-up-box">
                    <Link href={`/auth/login`}>{forgotPasswordTexts.BackToLogin}</Link>
                  </div>
                </Col>
              </Form>
            )}
          </Formik>
        </div>
      </LoginBoxWrapper>
    </div>
  );
};

export default ForgotPassword;
