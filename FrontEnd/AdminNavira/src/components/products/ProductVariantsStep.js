import React, { useState } from 'react';
import { Card, CardBody, Table, Button, Modal, ModalHeader, ModalBody, ModalFooter, Input, FormGroup, Label, Row, Col } from 'reactstrap';
import { Plus, Edit3, Copy, Trash2, X } from 'react-feather';

const ProductVariantsStep = ({ data, setData, errors, onStepComplete }) => {
  const [variants, setVariants] = useState(data.variants || []);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingVariant, setEditingVariant] = useState(null);
  const [combinationsModalOpen, setCombinationsModalOpen] = useState(false);
  const [combinationSelections, setCombinationSelections] = useState({});
  const [showDuplicateWarning, setShowDuplicateWarning] = useState(false);

  const attributes = data.attributes || [];

  const openModal = (variant = null) => {
    if (variant) {
      setEditingVariant({...variant});
    } else {
      setEditingVariant({
        id: Date.now(),
        attributes: {},
        sku: '',
        price: 0,
        cost: 0,
        status: true,
        stock: 0
      });
    }
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
    setEditingVariant(null);
  };

  const handleAttributeChange = (attrName, value) => {
    if (editingVariant) {
      const newAttributes = { ...editingVariant.attributes, [attrName]: value };
      setEditingVariant({
        ...editingVariant,
        attributes: newAttributes
      });
    }
  };

  const saveVariant = () => {
    if (!editingVariant.sku.trim()) {
      alert("SKU الزامی است.");
      return;
    }

    if (editingVariant.price < 0) {
      alert("قیمت باید بزرگتر یا مساوی صفر باشد.");
      return;
    }

    // Check for duplicate SKU
    const duplicateSku = variants.some(v => v.id !== editingVariant.id && v.sku === editingVariant.sku);
    if (duplicateSku) {
      alert("این SKU قبلاً استفاده شده است.");
      return;
    }

    // Check for duplicate attribute combination
    const duplicateCombo = variants.some(v => {
      if (v.id === editingVariant.id) return false; // Skip the current variant being edited
      return Object.keys(v.attributes).every(key => 
        v.attributes[key] === editingVariant.attributes[key]
      );
    });

    if (duplicateCombo) {
      alert("این ترکیب ویژگی‌ها قبلاً وجود دارد.");
      return;
    }

    const updatedVariants = editingVariant.id && variants.some(v => v.id === editingVariant.id)
      ? variants.map(v => v.id === editingVariant.id ? editingVariant : v)
      : [...variants, editingVariant];

    setVariants(updatedVariants);
    setData(prev => ({
      ...prev,
      variants: updatedVariants
    }));
    
    closeModal();
  };

  const deleteVariant = (id) => {
    if (confirm("آیا از حذف این Variant اطمینان دارید؟")) {
      const updatedVariants = variants.filter(v => v.id !== id);
      setVariants(updatedVariants);
      setData(prev => ({
        ...prev,
        variants: updatedVariants
      }));
    }
  };

  const copyVariant = (variant) => {
    const newVariant = {
      ...variant,
      id: Date.now(),
      sku: `${variant.sku}-COPY`
    };
    const updatedVariants = [...variants, newVariant];
    setVariants(updatedVariants);
    setData(prev => ({
      ...prev,
      variants: updatedVariants
    }));
  };

  const generateCombinations = () => {
    if (attributes.length === 0) {
      alert("ابتدا حداقل یک ویژگی اضافه کنید.");
      return;
    }

    // Initialize combination selections with first value of each attribute
    const initialSelections = {};
    attributes.forEach(attr => {
      if (attr.values.length > 0) {
        initialSelections[attr.name] = attr.values[0];
      }
    });
    setCombinationSelections(initialSelections);
    setCombinationsModalOpen(true);
  };

  const selectAllCombinations = () => {
    // Generate all possible combinations
    let allCombinations = [[]];
    
    attributes.forEach(attr => {
      const newCombinations = [];
      allCombinations.forEach(combo => {
        attr.values.forEach(value => {
          newCombinations.push([...combo, { [attr.name]: value }]);
        });
      });
      allCombinations = newCombinations;
    });

    // Convert to the right format and filter out existing ones
    const newVariants = allCombinations
      .map(combo => {
        const attrs = combo.reduce((acc, item) => ({ ...acc, ...item }), {});
        return attrs;
      })
      .filter(newAttrs => {
        // Check if this combination already exists
        return !variants.some(existing => {
          return Object.keys(existing.attributes).every(key => 
            existing.attributes[key] === newAttrs[key]
          );
        });
      })
      .map(attrs => ({
        id: Date.now() + Math.random(),
        attributes: attrs,
        sku: `AUTO-${Date.now()}-${Math.floor(Math.random() * 1000)}`,
        price: 0,
        cost: 0,
        status: true,
        stock: 0
      }));

    if (newVariants.length > 0) {
      const updatedVariants = [...variants, ...newVariants];
      setVariants(updatedVariants);
      setData(prev => ({
        ...prev,
        variants: updatedVariants
      }));
    }

    setCombinationsModalOpen(false);
  };

  const renderAttributeName = (attrName) => {
    // Simple conversion of common English attribute names to Persian
    const translations = {
      'color': 'رنگ',
      'size': 'سایز',
      'design': 'طرح',
      'style': 'سبک',
      'material': 'جنس'
    };
    return translations[attrName.toLowerCase()] || attrName;
  };

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h5>Variantهای محصول</h5>
        <p className="text-muted mb-0">هر Variant می‌تواند قیمت، SKU، تصویر و موجودی مستقل داشته باشد.</p>
      </div>

      <div className="d-flex justify-content-between mb-3">
        <Button color="primary" onClick={() => openModal()}>
          <Plus size={16} className="me-1" />
          ایجاد Variant
        </Button>
        <Button color="outline-secondary" onClick={generateCombinations}>
          <Plus size={16} className="me-1" />
          ایجاد Variantها به صورت ترکیبی
        </Button>
      </div>

      <div className="table-responsive">
        <Table className="table-hover">
          <thead className="table-light">
            <tr>
              <th>#</th>
              <th>ویژگی‌ها</th>
              <th>SKU</th>
              <th>قیمت فروش</th>
              <th>وضعیت</th>
              <th>عملیات</th>
            </tr>
          </thead>
          <tbody>
            {variants.map((variant, index) => (
              <tr key={variant.id}>
                <td>{index + 1}</td>
                <td>
                  {Object.entries(variant.attributes).map(([key, value], idx, arr) => (
                    <span key={key}>
                      {renderAttributeName(key)}: {value}{idx < arr.length - 1 ? ' / ' : ''}
                    </span>
                  ))}
                </td>
                <td>{variant.sku}</td>
                <td>{new Intl.NumberFormat('fa-IR').format(variant.price)} تومان</td>
                <td>
                  <span className={`badge ${variant.status ? 'bg-success' : 'bg-secondary'}`}>
                    {variant.status ? 'فعال' : 'غیرفعال'}
                  </span>
                </td>
                <td>
                  <Button 
                    color="outline-primary" 
                    size="sm" 
                    className="me-1"
                    onClick={() => openModal(variant)}
                  >
                    <Edit3 size={14} />
                  </Button>
                  <Button 
                    color="outline-info" 
                    size="sm" 
                    className="me-1"
                    onClick={() => copyVariant(variant)}
                  >
                    <Copy size={14} />
                  </Button>
                  <Button 
                    color="outline-danger" 
                    size="sm"
                    onClick={() => deleteVariant(variant.id)}
                  >
                    <Trash2 size={14} />
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      </div>

      {variants.length === 0 && (
        <div className="text-center py-5">
          <p className="text-muted">هیچ Variant ای ایجاد نشده است. دکمه "ایجاد Variant" را بزنید.</p>
        </div>
      )}

      {/* Edit/Create Variant Modal */}
      <Modal isOpen={modalOpen} toggle={closeModal} size="lg">
        <ModalHeader toggle={closeModal}>
          {editingVariant?.id ? 'ویرایش Variant' : 'ایجاد Variant جدید'}
        </ModalHeader>
        <ModalBody>
          {attributes.length > 0 ? (
            <Row>
              {attributes.map(attr => (
                <Col md="6" key={attr.id} className="mb-3">
                  <FormGroup>
                    <Label for={`attr-${attr.id}`}>{attr.name}</Label>
                    <Input
                      id={`attr-${attr.id}`}
                      type="select"
                      value={editingVariant?.attributes[attr.name] || ''}
                      onChange={(e) => handleAttributeChange(attr.name, e.target.value)}
                    >
                      <option value="">انتخاب کنید</option>
                      {attr.values.map(value => (
                        <option key={value} value={value}>{value}</option>
                      ))}
                    </Input>
                  </FormGroup>
                </Col>
              ))}
            </Row>
          ) : (
            <div className="alert alert-warning">ابتدا ویژگی‌های محصول را تعریف کنید.</div>
          )}

          <Row>
            <Col md="6" className="mb-3">
              <FormGroup>
                <Label for="sku">SKU</Label>
                <Input
                  id="sku"
                  type="text"
                  value={editingVariant?.sku || ''}
                  onChange={(e) => setEditingVariant({...editingVariant, sku: e.target.value})}
                  placeholder="مثلاً MUG-W-FL"
                />
              </FormGroup>
            </Col>
            <Col md="6" className="mb-3">
              <FormGroup>
                <Label for="price">قیمت فروش (تومان)</Label>
                <Input
                  id="price"
                  type="number"
                  value={editingVariant?.price || 0}
                  onChange={(e) => setEditingVariant({...editingVariant, price: parseInt(e.target.value) || 0})}
                  placeholder="0"
                />
              </FormGroup>
            </Col>
          </Row>

          <Row>
            <Col md="6" className="mb-3">
              <FormGroup>
                <Label for="cost">قیمت خرید (تومان)</Label>
                <Input
                  id="cost"
                  type="number"
                  value={editingVariant?.cost || 0}
                  onChange={(e) => setEditingVariant({...editingVariant, cost: parseInt(e.target.value) || 0})}
                  placeholder="0"
                />
              </FormGroup>
            </Col>
            <Col md="6" className="mb-3">
              <FormGroup>
                <Label for="stock">موجودی</Label>
                <Input
                  id="stock"
                  type="number"
                  value={editingVariant?.stock || 0}
                  onChange={(e) => setEditingVariant({...editingVariant, stock: parseInt(e.target.value) || 0})}
                  placeholder="0"
                />
              </FormGroup>
            </Col>
          </Row>

          <FormGroup>
            <Label for="status">وضعیت</Label>
            <div className="d-flex align-items-center">
              <div 
                className={`switch-handle ${editingVariant?.status ? 'active' : ''}`}
                onClick={() => setEditingVariant({...editingVariant, status: !editingVariant.status})}
                style={{
                  width: '50px',
                  height: '24px',
                  backgroundColor: editingVariant?.status ? '#0d6efd' : '#ced4da',
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
                    left: editingVariant?.status ? 'calc(100% - 22px)' : '2px',
                    transition: 'left 0.3s'
                  }}
                ></div>
              </div>
              <span className="ms-2">{editingVariant?.status ? 'فعال' : 'غیرفعال'}</span>
            </div>
          </FormGroup>
        </ModalBody>
        <ModalFooter>
          <Button color="secondary" onClick={closeModal}>لغو</Button>
          <Button color="primary" onClick={saveVariant}>
            {editingVariant?.id ? 'ذخیره Variant' : 'ایجاد Variant'}
          </Button>
        </ModalFooter>
      </Modal>

      {/* Combinations Modal */}
      <Modal isOpen={combinationsModalOpen} toggle={() => setCombinationsModalOpen(false)} size="lg">
        <ModalHeader toggle={() => setCombinationsModalOpen(false)}>ایجاد Variantهای ترکیبی</ModalHeader>
        <ModalBody>
          <p>برای هر ویژگی مقادیر مورد نظر را انتخاب کنید:</p>
          
          {attributes.map(attr => (
            <FormGroup key={attr.id} className="mb-3">
              <Label>{attr.name}</Label>
              <div>
                {attr.values.map(value => (
                  <Button
                    key={value}
                    color={combinationSelections[attr.name] === value ? 'primary' : 'outline-secondary'}
                    size="sm"
                    className="me-2 mb-2"
                    onClick={() => {
                      setCombinationSelections(prev => ({
                        ...prev,
                        [attr.name]: prev[attr.name] === value ? undefined : value
                      }));
                    }}
                  >
                    {value}
                  </Button>
                ))}
              </div>
            </FormGroup>
          ))}
          
          <div className="mt-4">
            <p>تعداد ترکیبات قابل ایجاد: {Object.keys(combinationSelections).filter(k => combinationSelections[k]).length > 0 ? 'محاسبه می‌شود...' : 0}</p>
          </div>
        </ModalBody>
        <ModalFooter>
          <Button color="secondary" onClick={() => setCombinationsModalOpen(false)}>لغو</Button>
          <Button color="primary" onClick={selectAllCombinations}>ایجاد همه ترکیبات</Button>
        </ModalFooter>
      </Modal>

      <div className="d-none"> {/* Hidden form for submission */}
        <input type="hidden" value={JSON.stringify(variants)} id="variantsData" />
      </div>
    </div>
  );
};

export default ProductVariantsStep;
