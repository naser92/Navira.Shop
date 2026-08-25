"use client";

import { useState } from "react";
import { Card, CardBody, Button, Row, Col, Container, Alert } from "reactstrap";
import { Save, X, Eye, CheckCircle, ArrowRight, ArrowLeft } from "react-feather";
import ProductBasicInfoStep from "@/components/products/ProductBasicInfoStep";
import ProductAttributesStep from "@/components/products/ProductAttributesStep";
import ProductVariantsStep from "@/components/products/ProductVariantsStep";
import ProductImagesStep from "@/components/products/ProductImagesStep";
import ProductInventoryStep from "@/components/products/ProductInventoryStep";
import ProductReviewStep from "@/components/products/ProductReviewStep";
import ProductStepper from "@/components/products/ProductStepper";

const STEPS = [
  { id: 1, title: "اطلاعات اصلی", component: ProductBasicInfoStep },
  { id: 2, title: "ویژگی‌ها", component: ProductAttributesStep },
  { id: 3, title: "Variantها", component: ProductVariantsStep },
  { id: 4, title: "تصاویر", component: ProductImagesStep },
  { id: 5, title: "موجودی", component: ProductInventoryStep },
];

const STEP_TITLES = [
  "ثبت محصول جدید",
  "ویژگی‌های محصول",
  "Variantهای محصول",
  "تصاویر محصول",
  "موجودی",
];

const STEP_SUBTITLES = [
  "ایجاد محصول، تعریف ویژگی‌ها، Variantها، تصاویر و موجودی",
  "ویژگی‌هایی را انتخاب کنید که Variantهای محصول بر اساس آن‌ها ساخته می‌شوند.",
  "هر Variant می‌تواند قیمت، SKU، تصویر و موجودی مستقل داشته باشد.",
  "تصاویر محصول و Variantها را بارگذاری کنید.",
  "موجودی اولیه برای انبارها تنظیم کنید.",
];

export default function ProductCreatePage() {
  const [currentStep, setCurrentStep] = useState(1);
  const [productData, setProductData] = useState({
    basicInfo: {
      name: "",
      sku: "",
      slug: "",
      category: "",
      brand: "",
      taxCategory: "",
      shortDescription: "",
      description: "",
      isActive: true,
      isPublished: true,
    },
    attributes: [
      { id: 1, name: "رنگ", values: ["سفید", "آبی", "صورتی", "بنفش"] },
      { id: 2, name: "طرح", values: ["گل", "دریا", "قلب", "پروانه"] },
    ],
    variants: [
      { id: 1, attributes: { color: "سفید", design: "گل" }, sku: "MUG-W-FL", price: 450000, cost: 400000, status: true, stock: 20 },
      { id: 2, attributes: { color: "آبی", design: "دریا" }, sku: "MUG-B-SE", price: 450000, cost: 400000, status: true, stock: 15 },
      { id: 3, attributes: { color: "صورتی", design: "قلب" }, sku: "MUG-P-HT", price: 470000, cost: 420000, status: true, stock: 10 },
      { id: 4, attributes: { color: "بنفش", design: "پروانه" }, sku: "MUG-P-BT", price: 470000, cost: 420000, status: true, stock: 8 },
    ],
    images: [],
    inventory: [
      { variantId: 1, initialStock: 20, minStock: 5, reserved: 0, available: 20 },
      { variantId: 2, initialStock: 15, minStock: 5, reserved: 0, available: 15 },
      { variantId: 3, initialStock: 10, minStock: 3, reserved: 0, available: 10 },
      { variantId: 4, initialStock: 8, minStock: 3, reserved: 0, available: 8 },
    ],
  });

  const [isReviewMode, setIsReviewMode] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errors, setErrors] = useState({});

  const handleNext = () => {
    if (currentStep < STEPS.length) {
      setCurrentStep(currentStep + 1);
    }
  };

  const handlePrevious = () => {
    if (currentStep > 1) {
      setCurrentStep(currentStep - 1);
    }
  };

  const handleStepSubmit = (stepData, stepIndex) => {
    setProductData(prev => ({
      ...prev,
      [`step${stepIndex}`]: stepData
    }));
    
    if (stepIndex < STEPS.length) {
      setCurrentStep(stepIndex + 1);
    } else {
      setIsReviewMode(true);
    }
  };

  const handleSaveDraft = () => {
    console.log("Draft saved:", productData);
    alert("پیش‌نویس ذخیره شد");
  };

  const handleCancel = () => {
    if (confirm("آیا از لغو ایجاد محصول اطمینان دارید؟")) {
      window.history.back();
    }
  };

  const handleSubmit = () => {
    setIsSubmitting(true);
    // Simulate API call
    setTimeout(() => {
      console.log("Product submitted:", {
        ...productData.basicInfo,
        attributes: productData.attributes,
        variants: productData.variants,
        images: productData.images,
        inventory: productData.inventory,
      });
      setIsSubmitting(false);
      alert("محصول با موفقیت ثبت شد");
    }, 1000);
  };

  const validateStep = (stepNumber) => {
    const newErrors = {};
    
    if (stepNumber === 1) {
      if (!productData.basicInfo.name.trim()) newErrors.name = "نام محصول الزامی است.";
      if (!productData.basicInfo.sku.trim()) newErrors.sku = "SKU الزامی است.";
      if (!productData.basicInfo.slug.trim()) newErrors.slug = "Slug الزامی است.";
      if (!productData.basicInfo.category) newErrors.category = "دسته‌بندی الزامی است.";
    } else if (stepNumber === 3) {
      if (productData.variants.length === 0) {
        newErrors.variants = "حداقل یک Variant ایجاد کنید.";
      }
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const canProceedToNext = (stepNumber) => {
    if (stepNumber === 1) {
      return !!productData.basicInfo.name.trim() && 
             !!productData.basicInfo.sku.trim() && 
             !!productData.basicInfo.slug.trim() && 
             !!productData.basicInfo.category;
    } else if (stepNumber === 3) {
      return productData.variants.length > 0;
    }
    return true;
  };

  const StepComponent = isReviewMode ? ProductReviewStep : STEPS[currentStep - 1]?.component;

  return (
    <div dir="rtl" className="py-4">
      <Container fluid>
        <Row>
          <Col xl="12">
            <div className="d-flex justify-content-between align-items-center mb-4">
              <div>
                <h2 className="mb-1">{STEP_TITLES[currentStep - 1]}</h2>
                <p className="text-muted mb-0">{STEP_SUBTITLES[currentStep - 1]}</p>
              </div>
              <div className="d-flex gap-2">
                <Button color="outline-secondary" onClick={handleSaveDraft}>
                  <Save size={16} className="me-1" />
                  ذخیره پیش‌نویس
                </Button>
                <Button color="outline-danger" onClick={handleCancel}>
                  <X size={16} className="me-1" />
                  انصراف
                </Button>
              </div>
            </div>

            <Card className="mb-4">
              <CardBody>
                {!isReviewMode && (
                  <ProductStepper currentStep={currentStep} />
                )}

                {errors[currentStep === 1 ? 'variants' : 'variants'] && (
                  <Alert color="danger">
                    {errors[currentStep === 1 ? 'variants' : 'variants']}
                  </Alert>
                )}

                <div className="mt-4">
                  {isReviewMode ? (
                    <ProductReviewStep 
                      data={productData} 
                      onSubmit={() => setIsReviewMode(false)}
                    />
                  ) : (
                    <StepComponent 
                      data={productData} 
                      setData={setProductData}
                      errors={errors}
                      onValidate={() => validateStep(currentStep)}
                      onStepComplete={(data) => handleStepSubmit(data, currentStep)}
                    />
                  )}
                </div>
              </CardBody>
            </Card>

            {/* Wizard Footer */}
            {!isReviewMode && (
              <div className="d-flex justify-content-between border-top pt-3">
                <Button 
                  color="outline-primary" 
                  onClick={handlePrevious}
                  disabled={currentStep === 1}
                >
                  <ArrowRight size={16} className="me-1" />
                  مرحله قبل
                </Button>
                
                {currentStep < STEPS.length ? (
                  <Button 
                    color="primary" 
                    onClick={() => {
                      if (validateStep(currentStep)) {
                        handleNext();
                      }
                    }}
                    disabled={!canProceedToNext(currentStep)}
                  >
                    ادامه
                    <ArrowLeft size={16} className="me-1" />
                  </Button>
                ) : (
                  <Button 
                    color="success" 
                    onClick={() => {
                      if (validateStep(currentStep)) {
                        setIsReviewMode(true);
                      }
                    }}
                    disabled={!canProceedToNext(currentStep)}
                  >
                    مرحله نهایی
                    <Eye size={16} className="me-1" />
                  </Button>
                )}
              </div>
            )}

            {isReviewMode && (
              <div className="d-flex justify-content-between border-top pt-3">
                <Button 
                  color="outline-secondary" 
                  onClick={() => setIsReviewMode(false)}
                >
                  <ArrowRight size={16} className="me-1" />
                  بازگشت و ویرایش
                </Button>
                <div className="d-flex gap-2">
                  <Button 
                    color="outline-warning" 
                    onClick={handleSaveDraft}
                  >
                    <Save size={16} className="me-1" />
                    ذخیره به عنوان پیش‌نویس
                  </Button>
                  <Button 
                    color="success" 
                    onClick={handleSubmit}
                    disabled={isSubmitting}
                  >
                    {isSubmitting ? "در حال ثبت..." : "ثبت محصول"}
                    <CheckCircle size={16} className="me-1" />
                  </Button>
                </div>
              </div>
            )}
          </Col>
        </Row>
      </Container>
    </div>
  );
}
