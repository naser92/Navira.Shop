import React from 'react';
import { Row, Col } from 'reactstrap';

const steps = [
  { id: 1, title: "اطلاعات اصلی" },
  { id: 2, title: "ویژگی‌ها" },
  { id: 3, title: "Variantها" },
  { id: 4, title: "تصاویر" },
  { id: 5, title: "موجودی" },
];

const ProductStepper = ({ currentStep }) => {
  return (
    <div className="mb-4">
      <div className="d-flex justify-content-center mb-4" style={{ maxWidth: '100%', overflowX: 'auto' }}>
        <div className="d-flex align-items-center w-100" style={{ justifyContent: 'space-between' }}>
          {steps.map((step, index) => (
            <React.Fragment key={step.id}>
              <div 
                className={`d-flex flex-column align-items-center px-2 py-2 rounded ${
                  currentStep === step.id 
                    ? 'bg-primary text-white' 
                    : currentStep > step.id 
                      ? 'bg-success text-white' 
                      : 'bg-light text-dark'
                }`}
                style={{ minWidth: '120px', cursor: currentStep > step.id ? 'pointer' : 'default' }}
              >
                <div className="d-flex align-items-center justify-content-center mb-1" 
                     style={{ width: '32px', height: '32px', borderRadius: '50%', backgroundColor: currentStep === step.id ? '#0d6efd' : currentStep > step.id ? '#198754' : '#6c757d', color: 'white', fontSize: '14px' }}>
                  {currentStep > step.id ? (
                    <i className="ri-check-line"></i>
                  ) : (
                    step.id
                  )}
                </div>
                <div className="text-center" style={{ fontSize: '12px', fontWeight: '500' }}>
                  {step.title}
                </div>
              </div>
              {index < steps.length - 1 && (
                <div 
                  className="flex-grow-1 mx-1" 
                  style={{ 
                    height: '2px', 
                    backgroundColor: currentStep > step.id ? '#198754' : '#dee2e6',
                    minHeight: '2px'
                  }}
                ></div>
              )}
            </React.Fragment>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ProductStepper;
