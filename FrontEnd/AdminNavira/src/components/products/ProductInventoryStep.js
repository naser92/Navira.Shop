import React, { useState } from 'react';
import { Card, CardBody, Table, Input, FormGroup, Label } from 'reactstrap';

const ProductInventoryStep = ({ data, setData, onStepComplete }) => {
  const [inventory, setInventory] = useState(data.inventory || []);
  const [warehouse, setWarehouse] = useState('1'); // Default to central warehouse

  const warehouses = [
    { id: '1', name: 'انبار مرکزی' },
    { id: '2', name: 'انبار شمال' },
    { id: '3', name: 'انبار جنوب' },
  ];

  const variants = data.variants || [];

  const handleInventoryChange = (variantId, field, value) => {
    const updatedInventory = inventory.map(item => {
      if (item.variantId === variantId) {
        const newItem = { ...item, [field]: parseInt(value) || 0 };
        // Calculate available stock
        newItem.available = newItem.initialStock - newItem.reserved;
        return newItem;
      }
      return item;
    });

    setInventory(updatedInventory);
    setData(prev => ({
      ...prev,
      inventory: updatedInventory
    }));
  };

  // Initialize inventory data if it's empty
  React.useEffect(() => {
    if (inventory.length === 0 && variants.length > 0) {
      const newInventory = variants.map(variant => ({
        variantId: variant.id,
        initialStock: 0,
        minStock: 0,
        reserved: 0,
        available: 0,
      }));
      setInventory(newInventory);
      setData(prev => ({
        ...prev,
        inventory: newInventory
      }));
    }
  }, [inventory, variants, setData]);

  const getVariantName = (variantId) => {
    const variant = variants.find(v => v.id === variantId);
    if (!variant) return 'نامشخص';
    
    return Object.entries(variant.attributes).map(([key, value]) => value).join(' / ');
  };

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h5>موجودی</h5>
      </div>

      <FormGroup className="mb-4">
        <Label for="warehouseSelect">انبار</Label>
        <Input
          type="select"
          id="warehouseSelect"
          value={warehouse}
          onChange={(e) => setWarehouse(e.target.value)}
        >
          {warehouses.map(w => (
            <option key={w.id} value={w.id}>{w.name}</option>
          ))}
        </Input>
      </FormGroup>

      <div className="table-responsive">
        <Table className="table-hover">
          <thead className="table-light">
            <tr>
              <th>Variant</th>
              <th>موجودی اولیه</th>
              <th>حداقل موجودی</th>
              <th>رزرو شده</th>
              <th>قابل فروش</th>
            </tr>
          </thead>
          <tbody>
            {variants.map((variant, index) => {
              const invItem = inventory.find(i => i.variantId === variant.id) || { 
                initialStock: 0, 
                minStock: 0, 
                reserved: 0, 
                available: 0 
              };
              
              return (
                <tr key={variant.id}>
                  <td>{getVariantName(variant.id)}</td>
                  <td>
                    <Input
                      type="number"
                      value={invItem.initialStock}
                      onChange={(e) => handleInventoryChange(variant.id, 'initialStock', e.target.value)}
                      className="w-auto"
                      style={{ width: '100px' }}
                    />
                  </td>
                  <td>
                    <Input
                      type="number"
                      value={invItem.minStock}
                      onChange={(e) => handleInventoryChange(variant.id, 'minStock', e.target.value)}
                      className="w-auto"
                      style={{ width: '100px' }}
                    />
                  </td>
                  <td>
                    <Input
                      type="number"
                      value={invItem.reserved}
                      readOnly
                      className="w-auto bg-light"
                      style={{ width: '100px' }}
                    />
                  </td>
                  <td>
                    <Input
                      type="number"
                      value={invItem.available}
                      readOnly
                      className="w-auto bg-light"
                      style={{ width: '100px' }}
                    />
                  </td>
                </tr>
              );
            })}
          </tbody>
        </Table>
      </div>

      <div className="alert alert-info mt-4">
        موجودی اولیه پس از ثبت محصول به عنوان موجودی اولیه ثبت خواهد شد.
      </div>

      <div className="d-none"> {/* Hidden form for submission */}
        <input type="hidden" value={JSON.stringify(inventory)} id="inventoryData" />
      </div>
    </div>
  );
};

export default ProductInventoryStep;
