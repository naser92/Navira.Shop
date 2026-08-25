import React, { useState } from 'react';
import { Card, CardBody, Row, Col, Input, Button, Badge, FormGroup, Label } from 'reactstrap';
import { Plus, X, Edit3 } from 'react-feather';

const ProductAttributesStep = ({ data, setData, onStepComplete }) => {
  const [attributes, setAttributes] = useState(data.attributes || []);
  const [showAddAttribute, setShowAddAttribute] = useState(false);
  const [newAttributeName, setNewAttributeName] = useState('');
  const [newAttributeValue, setNewAttributeValue] = useState('');
  const [editingAttribute, setEditingAttribute] = useState(null);
  const [editingValue, setEditingValue] = useState('');

  const handleAddAttribute = () => {
    if (newAttributeName.trim()) {
      const newAttribute = {
        id: Date.now(),
        name: newAttributeName.trim(),
        values: []
      };
      const updatedAttributes = [...attributes, newAttribute];
      setAttributes(updatedAttributes);
      setData(prev => ({
        ...prev,
        attributes: updatedAttributes
      }));
      setNewAttributeName('');
      setShowAddAttribute(false);
    }
  };

  const handleAddValue = (attributeId) => {
    const attrIndex = attributes.findIndex(attr => attr.id === attributeId);
    if (attrIndex !== -1 && newAttributeValue.trim()) {
      // Check for duplicates
      if (!attributes[attrIndex].values.includes(newAttributeValue.trim())) {
        const updatedAttributes = [...attributes];
        updatedAttributes[attrIndex] = {
          ...updatedAttributes[attrIndex],
          values: [...updatedAttributes[attrIndex].values, newAttributeValue.trim()]
        };
        setAttributes(updatedAttributes);
        setData(prev => ({
          ...prev,
          attributes: updatedAttributes
        }));
        setNewAttributeValue('');
      }
    }
  };

  const handleRemoveValue = (attributeId, value) => {
    if (confirm(`آیا از حذف مقدار "${value}" اطمینان دارید؟`)) {
      const updatedAttributes = attributes.map(attr => {
        if (attr.id === attributeId) {
          return {
            ...attr,
            values: attr.values.filter(v => v !== value)
          };
        }
        return attr;
      });
      setAttributes(updatedAttributes);
      setData(prev => ({
        ...prev,
        attributes: updatedAttributes
      }));
    }
  };

  const handleRemoveAttribute = (attributeId) => {
    if (confirm(`آیا از حذف ویژگی "${attributes.find(attr => attr.id === attributeId)?.name}" اطمینان دارید؟`)) {
      const updatedAttributes = attributes.filter(attr => attr.id !== attributeId);
      setAttributes(updatedAttributes);
      setData(prev => ({
        ...prev,
        attributes: updatedAttributes
      }));
    }
  };

  const startEditingAttribute = (attribute) => {
    setEditingAttribute(attribute);
    setEditingValue(attribute.name);
  };

  const saveEditedAttribute = () => {
    if (editingValue.trim() && editingAttribute) {
      const updatedAttributes = attributes.map(attr => {
        if (attr.id === editingAttribute.id) {
          return {
            ...attr,
            name: editingValue.trim()
          };
        }
        return attr;
      });
      setAttributes(updatedAttributes);
      setData(prev => ({
        ...prev,
        attributes: updatedAttributes
      }));
      setEditingAttribute(null);
      setEditingValue('');
    }
  };

  const cancelEditing = () => {
    setEditingAttribute(null);
    setEditingValue('');
  };

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h5>ویژگی‌های محصول</h5>
        <p className="text-muted mb-0">ویژگی‌هایی را انتخاب کنید که Variantهای محصول بر اساس آن‌ها ساخته می‌شوند.</p>
      </div>

      <Row>
        {attributes.map(attribute => (
          <Col lg="6" key={attribute.id} className="mb-4">
            <Card className="border p-3">
              <div className="d-flex justify-content-between align-items-center mb-3">
                {editingAttribute?.id === attribute.id ? (
                  <div className="d-flex align-items-center w-100">
                    <Input
                      type="text"
                      value={editingValue}
                      onChange={(e) => setEditingValue(e.target.value)}
                      className="me-2"
                      placeholder="نام ویژگی"
                    />
                    <Button size="sm" color="success" onClick={saveEditedAttribute}>
                      ذخیره
                    </Button>
                    <Button size="sm" color="secondary" onClick={cancelEditing} className="mx-1">
                      لغو
                    </Button>
                  </div>
                ) : (
                  <div className="d-flex justify-content-between align-items-center w-100">
                    <h6 className="mb-0">{attribute.name}</h6>
                    <div>
                      <Button 
                        size="sm" 
                        color="outline-secondary" 
                        onClick={() => startEditingAttribute(attribute)}
                        className="me-1"
                      >
                        <Edit3 size={14} />
                      </Button>
                      <Button 
                        size="sm" 
                        color="outline-danger" 
                        onClick={() => handleRemoveAttribute(attribute.id)}
                      >
                        <X size={14} />
                      </Button>
                    </div>
                  </div>
                )}
              </div>
              
              <div className="mb-2">
                {attribute.values.map(value => (
                  <Badge 
                    key={value} 
                    color="light" 
                    className="me-2 mb-2 border d-inline-flex align-items-center"
                  >
                    {value}
                    <Button 
                      size="sm" 
                      color="link" 
                      className="p-0 m-0 ms-1 text-danger"
                      onClick={() => handleRemoveValue(attribute.id, value)}
                    >
                      <X size={12} />
                    </Button>
                  </Badge>
                ))}
              </div>
              
              <div className="d-flex">
                <Input
                  type="text"
                  value={newAttributeValue}
                  onChange={(e) => setNewAttributeValue(e.target.value)}
                  placeholder="مقدار جدید"
                  onKeyDown={(e) => {
                    if (e.key === 'Enter') {
                      e.preventDefault();
                      handleAddValue(attribute.id);
                    }
                  }}
                />
                <Button 
                  color="primary" 
                  size="sm" 
                  className="ms-2"
                  onClick={() => handleAddValue(attribute.id)}
                >
                  افزودن مقدار
                </Button>
              </div>
            </Card>
          </Col>
        ))}

        {showAddAttribute ? (
          <Col lg="6" className="mb-4">
            <Card className="border p-3 bg-light">
              <FormGroup>
                <Label for="newAttributeName">نام ویژگی</Label>
                <Input
                  id="newAttributeName"
                  type="text"
                  value={newAttributeName}
                  onChange={(e) => setNewAttributeName(e.target.value)}
                  placeholder="نام ویژگی جدید"
                />
                <div className="d-flex mt-2">
                  <Button 
                    color="success" 
                    size="sm" 
                    className="me-2"
                    onClick={handleAddAttribute}
                  >
                    افزودن ویژگی
                  </Button>
                  <Button 
                    color="secondary" 
                    size="sm"
                    onClick={() => {
                      setShowAddAttribute(false);
                      setNewAttributeName('');
                    }}
                  >
                    لغو
                  </Button>
                </div>
              </FormGroup>
            </Card>
          </Col>
        ) : (
          <Col lg="6" className="mb-4">
            <Button 
              color="outline-primary" 
              className="w-100 h-100 d-flex flex-column align-items-center justify-content-center"
              onClick={() => setShowAddAttribute(true)}
            >
              <Plus size={20} className="mb-2" />
              افزودن ویژگی
            </Button>
          </Col>
        )}
      </Row>
      
      <div className="d-none"> {/* Hidden form for submission */}
        <input type="hidden" value={JSON.stringify(attributes)} id="attributesData" />
      </div>
    </div>
  );
};

export default ProductAttributesStep;
