import React from 'react';
import { Card, CardBody, Row, Col, Badge, Table } from 'reactstrap';
import { Image } from 'react-feather';

const ProductReviewStep = ({ data, onSubmit }) => {
  const { basicInfo, attributes, variants, images, inventory } = data;

  // Sample data for preview
  const warehouses = [
    { id: '1', name: 'انبار مرکزی' },
  ];

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

  const getCategoryName = (categoryId) => {
    const category = categories.find(c => c.id == categoryId);
    return category ? category.name : 'نامشخص';
  };

  const getBrandName = (brandId) => {
    const brand = brands.find(b => b.id == brandId);
    return brand ? brand.name : 'نامشخص';
  };

  const getImagePreview = () => {
    const primaryImage = images.find(img => img.isPrimary);
    return primaryImage ? (
      <img 
        src={primaryImage.url} 
        alt="Primary product" 
        className="rounded border"
        style={{ maxWidth: '200px', maxHeight: '200px' }}
      />
    ) : (
      <div className="border rounded p-4 text-center bg-light" style={{ width: '200px', height: '200px' }}>
        <Image size={48} className="text-muted" />
        <p className="mt-2">تصویری وجود ندارد</p>
      </div>
    );
  };

  return (
    <div>
      <h4 className="mb-4">مرور نهایی محصول</h4>
      
      <Card className="mb-4">
        <CardBody>
          <Row>
            <Col lg="4" className="d-flex justify-content-center">
              {getImagePreview()}
            </Col>
            <Col lg="8">
              <h3>{basicInfo.name || 'نام محصول'}</h3>
              
              <div className="mb-3">
                <p><strong>دسته‌بندی:</strong> {getCategoryName(basicInfo.category)}</p>
                <p><strong>برند:</strong> {getBrandName(basicInfo.brand)}</p>
                <p><strong>SKU:</strong> {basicInfo.sku}</p>
                <p><strong>Slug:</strong> {basicInfo.slug}</p>
                <p><strong>وضعیت:</strong> 
                  <Badge color={basicInfo.isActive ? "success" : "secondary"} className="ms-2">
                    {basicInfo.isActive ? "فعال" : "غیرفعال"}
                  </Badge>
                  <Badge color={basicInfo.isPublished ? "success" : "secondary"} className="ms-2">
                    {basicInfo.isPublished ? "منتشر شده" : "پیش‌نویس"}
                  </Badge>
                </p>
              </div>
              
              <p><strong>توضیح کوتاه:</strong> {basicInfo.shortDescription || '-'}</p>
            </Col>
          </Row>
        </CardBody>
      </Card>

      <Row>
        <Col lg="6">
          <Card className="mb-4">
            <CardBody>
              <h5 className="mb-3">ویژگی‌ها</h5>
              {attributes.length > 0 ? (
                <div>
                  {attributes.map(attr => (
                    <div key={attr.id} className="mb-3">
                      <h6>{attr.name}:</h6>
                      <div className="d-flex flex-wrap gap-2">
                        {attr.values.map(value => (
                          <Badge key={value} color="light" className="border">{value}</Badge>
                        ))}
                      </div>
                    </div>
                  ))}
                </div>
              ) : (
                <p className="text-muted">ویژگی‌ای تعریف نشده است</p>
              )}
            </CardBody>
          </Card>
        </Col>
        
        <Col lg="6">
          <Card className="mb-4">
            <CardBody>
              <h5 className="mb-3">اطلاعات بیشتر</h5>
              <p><strong>تعداد تصاویر:</strong> {images.length}</p>
              <p><strong>تعداد Variantها:</strong> {variants.length}</p>
              <p><strong>موجودی کل:</strong> {inventory.reduce((sum, inv) => sum + inv.initialStock, 0)}</p>
            </CardBody>
          </Card>
        </Col>
      </Row>

      <Card className="mb-4">
        <CardBody>
          <h5 className="mb-3">Variantها</h5>
          <div className="table-responsive">
            <Table className="table-sm">
              <thead className="table-light">
                <tr>
                  <th>ویژگی‌ها</th>
                  <th>SKU</th>
                  <th>قیمت</th>
                  <th>موجودی</th>
                </tr>
              </thead>
              <tbody>
                {variants.map(variant => (
                  <tr key={variant.id}>
                    <td>
                      {Object.entries(variant.attributes).map(([key, value], idx, arr) => (
                        <span key={key}>
                          {value}{idx < arr.length - 1 ? ' / ' : ''}
                        </span>
                      ))}
                    </td>
                    <td>{variant.sku}</td>
                    <td>{new Intl.NumberFormat('fa-IR').format(variant.price)} تومان</td>
                    <td>{variant.stock || '0'} عدد</td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </div>
        </CardBody>
      </Card>

      <div className="alert alert-success">
        <h5 className="alert-heading">آماده ثبت!</h5>
        <p>تمام اطلاعات محصول تکمیل شده است. می‌توانید محصول را ثبت کنید یا به مراحل قبلی برگردید تا تغییراتی اعمال کنید.</p>
      </div>
    </div>
  );
};

export default ProductReviewStep;
