"use client";

import { ReactstrapInput } from "@/components/reactstrapFormik";
import ShowBox from "@/elements/alerts&Modals/ShowBox";
import Btn from "@/elements/buttons/Btn";
import SettingContext from "@/helper/settingContext";
import LoginBoxWrapper from "@/utils/hoc/LoginBoxWrapper";
import { YupObject, nameSchema } from "@/utils/validation/ValidationSchemas";
import { ErrorMessage, Field, Form, Formik } from "formik";
import Image from "next/image";
import Link from "next/link";
import { useContext, useRef, useState } from "react";
import ReCAPTCHA from "react-google-recaptcha";
import { Col } from "reactstrap";
import { useRouter } from "next/navigation";
import { apiFetch } from "@/lib/api/clientApi";
import useAccount from "@/helper/accountContext/useAccount";

const loginTexts = {
  LogInYourAccount: "ورود به حساب کاربری",
  Username: "نام کاربری",
  Password: "رمز عبور",
  ForgotPassword: "رمز عبور خود را فراموش کرده‌اید؟",
  Login: "ورود",
  Logging: "...در حال ورود",
  DontHaveSellerAccount: "حساب فروشندگی ندارید؟",
  SignUp: "ثبت‌نام",
  LoginSuccessful: "ورود با موفقیت انجام شد",
  InvalidCredentials: "نام کاربری یا رمز عبور اشتباه است",
};

const Login = () => {
  const [showBoxMessage, setShowBoxMessage] = useState();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const { settingObj, state } = useContext(SettingContext);
  const reCaptchaRef = useRef();
  const router = useRouter();
  const { storeTokens, refreshProfile } = useAccount();

  const handleLogin = async (values) => {
    try {
      setIsSubmitting(true);
      setShowBoxMessage(undefined);

      const result = await apiFetch("/api/auth/login", {
        method: "POST",
        body: JSON.stringify({
          username: values.username,
          password: values.password,
        }),
      });

      storeTokens({
        accessToken: result?.data?.accessToken ?? null,
        refreshToken: result?.data?.refreshToken ?? null,
      });

      setShowBoxMessage({
        message: loginTexts.LoginSuccessful,
        type: "success",
      });

      await refreshProfile();
      router.replace("/dashboard");
      router.refresh();
    } catch (error) {
      setShowBoxMessage({
        message: error.message || loginTexts.InvalidCredentials,
        type: "danger",
      });

      if (settingObj?.google_reCaptcha?.status && reCaptchaRef.current) {
        reCaptchaRef.current.reset();
      }
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
          <h4>{loginTexts.LogInYourAccount}</h4>
        </div>

        <div className="input-box">
          <Formik
            initialValues={{
              username: "",
              password: "",
              recaptcha: "",
            }}
            validationSchema={YupObject({
              username: nameSchema,
              password: nameSchema,
            })}
            onSubmit={handleLogin}
          >
            {({ errors, touched, setFieldValue }) => (
              <Form className="row g-4" dir="rtl">
                <Col sm="12">
                  <Field
                    inputprops={{ noExtraSpace: true }}
                    autoComplete={true}
                    name="username"
                    type="text"
                    component={ReactstrapInput}
                    className="form-control"
                    id="username"
                    placeholder={loginTexts.Username}
                    label={loginTexts.Username}
                  />
                </Col>

                <Col sm="12">
                  <Field
                    inputprops={{ noExtraSpace: true }}
                    name="password"
                    component={ReactstrapInput}
                    type="password"
                    className="form-control"
                    id="password"
                    placeholder={loginTexts.Password}
                    label={loginTexts.Password}
                  />
                </Col>

                {settingObj?.google_reCaptcha?.status && (
                  <Col sm="12">
                    <ReCAPTCHA
                      ref={reCaptchaRef}
                      sitekey={settingObj?.google_reCaptcha?.site_key}
                      onChange={(value) => {
                        setFieldValue("recaptcha", value);
                      }}
                    />
                    {errors.recaptcha && touched.recaptcha && (
                      <ErrorMessage
                        name="recaptcha"
                        render={() => <div className="invalid-feedback d-block">{errors.recaptcha}</div>}
                      />
                    )}
                  </Col>
                )}

                <Col sm="12">
                  <div className="forgot-box">
                    <Link href={`/auth/forgot-password`} className="forgot-password">
                      {loginTexts.ForgotPassword}
                    </Link>
                  </div>
                </Col>

                <Col sm="12">
                  <Btn
                    title={isSubmitting ? loginTexts.Logging : loginTexts.Login}
                    className="btn btn-animation w-100 justify-content-center"
                    type="submit"
                    color="false"
                    disabled={isSubmitting}
                  />

                  <div className="sign-up-box">
                    <h4>{loginTexts.DontHaveSellerAccount}</h4>
                    <Link href={`/auth/register`}>{loginTexts.SignUp}</Link>
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

export default Login;
