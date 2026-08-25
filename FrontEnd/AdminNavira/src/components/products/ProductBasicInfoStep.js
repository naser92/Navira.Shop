import React, { useState } from 'react';
import { Card, CardBody, Row, Col, Input, Label, FormGroup, FormFeedback } from 'reactstrap';

const ProductBasicInfoStep = ({ data, setData, errors, onValidate, onStepComplete }) => {
  const [basicInfo, setBasicInfo] = useState(data.basicInfo || {
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
  });

  const categories = [
    { id: 1, name: 'ماگ' },
    { id: 2, name: 'پوشه' },
    { id: 3, name: 'کیف' },
    { id: 4, name: 'پیراهن' },
    { id: 5, name: 'کلاه' },
  ];

  const brands = [
    { id: 1, name: 'استندلی' },
    { id: 2, name: 'نایک' },
    { id: 3, name: 'اپل' },
    { id: 4, name: 'سامسونگ' },
    { id: 5, name: 'لنوو' },
  ];

  const taxCategories = [
    { id: 1, name: 'معاف از مالیات' },
    { id: 2, name: 'مالیات کالاهای معمولی' },
    { id: 3, name: 'مالیات کالاهای لوکس' },
  ];

  const handleChange = (field, value) => {
    setBasicInfo(prev => ({
      ...prev,
      [field]: value
    }));

    // Update parent state
    setData(prev => ({
      ...prev,
      basicInfo: {
        ...prev.basicInfo,
        [field]: value
      }
    }));
  };

  const handleSwitchChange = (field) => {
    const newValue = !basicInfo[field];
    setBasicInfo(prev => ({
      ...prev,
      [field]: newValue
    }));

    setData(prev => ({
      ...prev,
      basicInfo: {
        ...prev.basicInfo,
        [field]: newValue
      }
    }));
  };

  const handleSubmit = () => {
    onStepComplete(basicInfo);
  };

  return (
    <div>
      <Row>
        <Col lg="8">
          <Card className="mb-4">
            <CardBody>
              <h5 className="mb-4">اطلاعات محصول</h5>
              
              <Row>
                <Col md="6">
                  <FormGroup>
                    <Label for="name" className="required">نام محصول</Label>
                    <Input
                      id="name"
                      name="name"
                      type="text"
                      placeholder="مثلاً ماگ استندلی"
                      value={basicInfo.name}
                      onChange={(e) => handleChange('name', e.target.value)}
                      invalid={!!errors.name}
                    />
                    {errors.name && <FormFeedback>{errors.name}</FormFeedback>}
                  </FormGroup>
                </Col>
                
                <Col md="6">
                  <FormGroup>
                    <Label for="sku" className="required">SKU</Label>
                    <Input
                      id="sku"
                      name="sku"
                      type="text"
                      placeholder="MUG-STANLEY"
                      value={basicInfo.sku}
                      onChange={(e) => handleChange('sku', e.target.value)}
                      invalid={!!errors.sku}
                    />
                    {errors.sku && <FormFeedback>{errors.sku}</FormFeedback>}
                  </FormGroup>
                </Col>
              </Row>
              
              <Row>
                <Col md="6">
                  <FormGroup>
                    <Label for="slug" className="required">Slug</Label>
                    <Input
                      id="slug"
                      name="slug"
                      type="text"
                      placeholder="mug-stanley"
                      value={basicInfo.slug}
                      onChange={(e) => handleChange('slug', e.target.value)}
                      invalid={!!errors.slug}
                    />
                    {errors.slug && <FormFeedback>{errors.slug}</FormFeedback>}
                  </FormGroup>
                </Col>
                
                <Col md="6">
                  <FormGroup>
                    <Label for="category" className="required">دسته‌بندی</Label>
                    <Input
                      id="category"
                      name="category"
                      type="select"
                      value={basicInfo.category}
                      onChange={(e) => handleChange('category', e.target.value)}
                      invalid={!!errors.category}
                    >
                      <option value="">انتخاب کنید</option>
                      {categories.map(cat => (
                        <option key={cat.id} value={cat.id}>{cat.name}</option>
                      ))}
                    </Input>
                    {errors.category && <FormFeedback>{errors.category}</FormFeedback>}
                  </FormGroup>
                </Col>
              </Row>
              
              <Row>
                <Col md="6">
                  <FormGroup>
                    <Label for="brand">برند</Label>
                    <Input
                      id="brand"
                      name="brand"
                      type="select"
                      value={basicInfo.brand}
                      onChange={(e) => handleChange('brand', e.target.value)}
                    >
                      <option value="">انتخاب کنید</option>
                      {brands.map(brand => (
                        <option key={brand.id} value={brand.id}>{brand.name}</option>
                      ))}
                    </Input>
                  </FormGroup>
                </Col>
                
                <Col md="6">
                  <FormGroup>
                    <Label for="taxCategory">دسته مالیاتی</Label>
                    <Input
                      id="taxCategory"
                      name="taxCategory"
                      type="select"
                      value={basicInfo.taxCategory}
                      onChange={(e) => handleChange('taxCategory', e.target.value)}
                    >
                      <option value="">انتخاب کنید</option>
                      {taxCategories.map(tax => (
                        <option key={tax.id} value={tax.id}>{tax.name}</option>
                      ))}
                    </Input>
                  </FormGroup>
                </Col>
              </Row>
            </CardBody>
          </Card>
          
          <Card className="mb-4">
            <CardBody>
              <h5 className="mb-4">توضیحات</h5>
              
              <FormGroup>
                <Label for="shortDescription">توضیح کوتاه</Label>
                <Input
                  id="shortDescription"
                  name="shortDescription"
                  type="textarea"
                  rows="3"
                  placeholder="توضیح مختصر در مورد محصول..."
                  value={basicInfo.shortDescription}
                  onChange={(e) => handleChange('shortDescription', e.target.value)}
                />
              </FormGroup>
              
              <FormGroup className="mt-3">
                <Label for="description">توضیح کامل</Label>
                <Input
                  id="description"
                  name="description"
                  type="textarea"
                  rows="5"
                  placeholder="توضیح کامل در مورد محصول..."
                  value={basicInfo.description}
                  onChange={(e) => handleChange('description', e.target.value)}
                />
              </FormGroup>
            </CardBody>
          </Card>
        </Col>
        
        <Col lg="4">
          <Card className="mb-4">
            <CardBody>
              <h5 className="mb-4">وضعیت</h5>
              
              <FormGroup className="d-flex align-items-center justify-content-between p-3 border rounded mb-3">
                <Label for="isActive" className="mb-0">فعال است</Label>
                <div 
                  className={`switch-handle ${basicInfo.isActive ? 'active' : ''}`}
                  onClick={() => handleSwitchChange('isActive')}
                  style={{
                    width: '50px',
                    height: '24px',
                    backgroundColor: basicInfo.isActive ? '#0d6efd' : '#ced4da',
                    borderRadius: '12px',
                    position: 'relative',
                    cursor: 'pointer',
                    transition: 'background-color 0.3s'
                  }}
                >
                  <div 
                    style={{
                      width: '20px',
                      height: '20px',
                      backgroundColor: 'white',
                      borderRadius: '50%',
                      position: 'absolute',
                      top: '2px',
                      left: basicInfo.isActive ? 'calc(100% - 22px)' : '2px',
                      transition: 'left 0.3s'
                    }}
                  ></div>
                </div>
              </FormGroup>
              
              <FormGroup className="d-flex align-items-center justify-content-between p-3 border rounded">
                <Label for="isPublished" className="mb-0">منتشر شده است</Label>
                <div 
                  className={`switch-handle ${basicInfo.isPublished ? 'active' : ''}`}
                  onClick={() => handleSwitchChange('isPublished')}
                  style={{
                    width: '50px',
                    height: '24px',
                    backgroundColor: basicInfo.isPublished ? '#0d6efd' : '#ced4da',
                    borderRadius: '12px',
                    position: 'relative',
                    cursor: 'pointer',
                    transition: 'background-color 0.3s'
                  }}
                >
                  <div 
                    style={{
                      width: '20px',
                      height: '20px',
                      backgroundColor: 'white',
                      borderRadius: '50%',
                      position: 'absolute',
                      top: '2px',
                      left: basicInfo.isPublished ? 'calc(100% - 22px)' : '2px',
                      transition: 'left 0.3s'
                    }}
                  ></div>
                </div>
              </FormGroup>
            </CardBody>
          </Card>
        </Col>
      </Row>
      
      <div className="d-none"> {/* Hidden form for submission */}
        <input type="hidden" value={JSON.stringify(basicInfo)} id="basicInfoData" />
      </div>
    </div>
  );
};

export default ProductBasicInfoStep;
