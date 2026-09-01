import { useState } from "react";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader, Input, FormGroup, Label } from "reactstrap";
import { FaArrowUp, FaArrowDown } from "react-icons/fa";
import { clampWidth } from "../core/column-utils.js";

export function GridColumnManager({ columns, order, visible, widths, setOrder, setVisible, setWidths, localization }) {
  const [isOpen, setIsOpen] = useState(false);

  const toggle = () => setIsOpen(!isOpen);

  const toggleColumnVisibility = (field) => {
    const newVisible = new Set(visible);
    if (newVisible.has(field)) {
      newVisible.delete(field);
    } else {
      newVisible.add(field);
    }
    setVisible(newVisible);
  };

  const moveColumn = (index, direction) => {
    const newOrder = [...order];
    const targetIndex = index + direction;
    if (targetIndex < 0 || targetIndex >= newOrder.length) return;
    
    [newOrder[index], newOrder[targetIndex]] = [newOrder[targetIndex], newOrder[index]];
    setOrder(newOrder);
  };

  const updateWidth = (field, width) => {
    const newWidths = { ...widths, [field]: clampWidth(width) };
    setWidths(newWidths);
  };

  const restoreDefaults = () => {
    setOrder(columns.map(c => c.field));
    setVisible(new Set(columns.filter(c => c.visible !== false).map(c => c.field)));
    setWidths(columns.reduce((acc, col) => {
      if (col.width) acc[col.field] = col.width;
      return acc;
    }, {}));
  };

  return (
    <>
      <Button size="sm" color="outline-secondary" onClick={toggle}>
        {localization.columns}
      </Button>

      <Modal isOpen={isOpen} toggle={toggle}>
        <ModalHeader toggle={toggle}>{localization.columns}</ModalHeader>
        <ModalBody>
          {order.map((field, index) => {
            const column = columns.find(c => c.field === field);
            if (!column) return null;

            return (
              <div key={field} className="navira-grid-column-manager-row">
                <FormGroup check>
                  <Input
                    type="checkbox"
                    id={`column-${field}`}
                    checked={visible.has(field)}
                    onChange={() => toggleColumnVisibility(field)}
                  />
                  <Label for={`column-${field}`}>
                    {column.title}
                  </Label>
                </FormGroup>

                <div className="navira-grid-column-controls">
                  <Button
                    size="sm"
                    disabled={index === 0}
                    onClick={() => moveColumn(index, -1)}
                  >
                    <FaArrowUp />
                  </Button>
                  <Button
                    size="sm"
                    disabled={index === order.length - 1}
                    onClick={() => moveColumn(index, 1)}
                  >
                    <FaArrowDown />
                  </Button>

                  <Input
                    type="number"
                    value={widths[field] || ''}
                    onChange={(e) => updateWidth(field, e.target.value ? Number(e.target.value) : undefined)}
                    placeholder="Width"
                    min="60"
                    max="600"
                  />
                </div>
              </div>
            );
          })}
        </ModalBody>
        <ModalFooter>
          <Button color="secondary" onClick={restoreDefaults}>
            {localization.restoreDefaults}
          </Button>
          <Button color="primary" onClick={toggle}>
            {localization.apply}
          </Button>
        </ModalFooter>
      </Modal>
    </>
  );
}
