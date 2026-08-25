import React, { useState } from 'react';
import { Card, CardBody, Row, Col, Button, Input, InputGroup, InputGroupText, Badge } from 'reactstrap';
import { Upload, X, Star, Move } from 'react-feather';

const ProductImagesStep = ({ data, setData, onStepComplete }) => {
  const [productImages, setProductImages] = useState(data.images || []);
  const [variantImages, setVariantImages] = useState({});

  // Mock variant names based on the example
  const variants = data.variants || [];

  const handleImageUpload = (e, isVariant = false, variantId = null) => {
    const files = Array.from(e.target.files);
    
    const newImages = files.map(file => ({
      id: Date.now() + Math.random(),
      file: file,
      url: URL.createObjectURL(file),
      name: file.name,
      size: file.size
    }));

    if (isVariant) {
      setVariantImages(prev => ({
        ...prev,
        [variantId]: [...(prev[variantId] || []), ...newImages]
      }));
    } else {
      setProductImages(prev => [...prev, ...newImages]);
    }
  };

  const removeImage = (imageId, isVariant = false, variantId = null) => {
    if (isVariant) {
      setVariantImages(prev => ({
        ...prev,
        [variantId]: prev[variantId].filter(img => img.id !== imageId)
      }));
    } else {
      setProductImages(prev => prev.filter(img => img.id !== imageId));
    }
  };

  const setPrimaryImage = (imageId) => {
    const updatedImages = productImages.map(img => ({
      ...img,
      isPrimary: img.id === imageId
    }));
    setProductImages(updatedImages);
  };

  const reorderImages = (fromIndex, toIndex, isVariant = false, variantId = null) => {
    let images = isVariant ? variantImages[variantId] || [] : productImages;
    
    if (images.length <= 1) return;
    
    const newImages = [...images];
    const [movedItem] = newImages.splice(fromIndex, 1);
    newImages.splice(toIndex, 0, movedItem);
    
    if (isVariant) {
      setVariantImages(prev => ({
        ...prev,
        [variantId]: newImages
      }));
    } else {
      setProductImages(newImages);
    }
  };

  const formatFileSize = (bytes) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  };

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h5>تصاویر محصول</h5>
        <p className="text-muted mb-0">تصاویر محصول و Variantها را بارگذاری کنید.</p>
      </div>

      <Row>
        {/* Product Images Section */}
        <Col lg="6" className="mb-4">
          <Card className="border">
            <CardBody>
              <h6 className="mb-3">تصاویر اصلی محصول</h6>
              <div className="border-2 border-dashed rounded p-4 text-center mb-3">
                <Upload size={24} className="text-muted mb-2" />
                <p className="mb-1">تصویر را اینجا بکشید یا برای انتخاب کلیک کنید</p>
                <p className="text-muted mb-0" style={{ fontSize: '0.8rem' }}>PNG, JPG یا GIF (حداکثر 5MB)</p>
                <Input
                  type="file"
                  className="d-none"
                  id="productImageUpload"
                  multiple
                  accept="image/*"
                  onChange={handleImageUpload}
                />
                <Button 
                  color="primary" 
                  outline 
                  className="mt-2"
                  onClick={() => document.getElementById('productImageUpload').click()}
                >
                  انتخاب تصویر
                </Button>
              </div>

              <div className="d-flex flex-wrap gap-2">
                {productImages.map((img, index) => (
                  <div key={img.id} className="position-relative border rounded" style={{ width: '120px', height: '120px' }}>
                    <img 
                      src={img.url} 
                      alt={`Product ${index + 1}`} 
                      className="w-100 h-100 object-fit-cover rounded"
                      style={{ maxHeight: '120px' }}
                    />
                    <Button 
                      color="danger" 
                      size="sm" 
                      className="position-absolute top-0 start-100 translate-middle"
                      onClick={() => removeImage(img.id)}
                    >
                      <X size={12} />
                    </Button>
                    <Button 
                      color={img.isPrimary ? "warning" : "light"} 
                      size="sm" 
                      className="position-absolute bottom-0 start-0 m-1"
                      onClick={() => setPrimaryImage(img.id)}
                      title="تنظیم به عنوان تصویر اصلی"
                    >
                      <Star size={12} fill={img.isPrimary ? "#ffc107" : "none"} />
                    </Button>
                    <div className="position-absolute top-0 start-0 m-1">
                      <Badge color="light" className="text-dark" style={{ fontSize: '0.6rem' }}>
                        {index + 1}
                      </Badge>
                    </div>
                  </div>
                ))}
              </div>
              {productImages.length > 0 && (
                <div className="mt-2">
                  <small className="text-muted">
                    {productImages.find(img => img.isPrimary) 
                      ? `تصویر اصلی: ${productImages.find(img => img.isPrimary).name}`
                      : 'هیچ تصویر اصلی انتخاب نشده است'}
                  </small>
                </div>
              )}
            </CardBody>
          </Card>
        </Col>

        {/* Variant Images Section */}
        <Col lg="6" className="mb-4">
          <Card className="border">
            <CardBody>
              <h6 className="mb-3">تصاویر Variantها</h6>
              {variants.length > 0 ? (
                <div className="d-flex flex-column gap-3">
                  {variants.map(variant => (
                    <div key={variant.id} className="border rounded p-3">
                      <div className="d-flex justify-content-between align-items-center mb-2">
                        <h6 className="mb-0">
                          {Object.entries(variant.attributes).map(([key, value], idx, arr) => (
                            <span key={key}>
                              {value}{idx < arr.length - 1 ? ' / ' : ''}
                            </span>
                          ))}
                        </h6>
                        <InputGroup size="sm" className="w-auto">
                          <InputGroupText>
                            <Upload size={14} />
                          </InputGroupText>
                          <Input
                            type="file"
                            className="form-control"
                            multiple
                            accept="image/*"
                            onChange={(e) => handleImageUpload(e, true, variant.id)}
                          />
                        </InputGroup>
                      </div>
                      
                      <div className="d-flex flex-wrap gap-2">
                        {(variantImages[variant.id] || []).map((img, idx) => (
                          <div key={img.id} className="position-relative border rounded" style={{ width: '80px', height: '80px' }}>
                            <img 
                              src={img.url} 
                              alt={`Variant ${idx + 1}`} 
                              className="w-100 h-100 object-fit-cover rounded"
                              style={{ maxHeight: '80px' }}
                            />
                            <Button 
                              color="danger" 
                              size="sm" 
                              className="position-absolute top-0 start-100 translate-middle"
                              onClick={() => removeImage(img.id, true, variant.id)}
                            >
                              <X size={10} />
                            </Button>
                          </div>
                        ))}
                      </div>
                    </div>
                  ))}
                </div>
              ) : (
                <div className="alert alert-info">
                  ابتدا Variantهای محصول را ایجاد کنید تا بتوانید تصاویر آنها را بارگذاری کنید.
                </div>
              )}
            </CardBody>
          </Card>
        </Col>
      </Row>

      <div className="d-none"> {/* Hidden form for submission */}
        <input type="hidden" value={JSON.stringify(productImages)} id="productImagesData" />
      </div>
    </div>
  );
};

export default ProductImagesStep;
