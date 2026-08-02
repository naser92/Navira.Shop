"use client";

import { useEffect, useState } from "react";
import { FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import Btn from "@/elements/buttons/Btn";
import { getItemId, getItemName } from "./accessUtils";

const ConnectionsModal = ({
  isOpen,
  toggle,
  title,
  description,
  options = [],
  selectedIds = [],
  isSaving = false,
  onSave,
}) => {
  const [checked, setChecked] = useState([]);

  useEffect(() => {
    if (isOpen) setChecked(selectedIds.map(String));
  }, [isOpen, selectedIds]);

  const toggleId = (id) => {
    const key = String(id);
    setChecked((prev) =>
      prev.includes(key) ? prev.filter((item) => item !== key) : [...prev, key]
    );
  };

  return (
    <Modal isOpen={isOpen} toggle={toggle} centered className="navira-access-modal">
      <ModalHeader toggle={toggle}>{title}</ModalHeader>
      <ModalBody>
        {description && <p className="navira-access-modal-desc">{description}</p>}
        {options.length === 0 ? (
          <p className="navira-access-empty">موردی برای اتصال وجود ندارد.</p>
        ) : (
          <div className="navira-access-options">
            {options.map((option) => {
              const id = getItemId(option);
              return (
                <FormGroup check key={id} className="navira-access-option">
                  <Input
                    type="checkbox"
                    id={`conn-${id}`}
                    checked={checked.includes(String(id))}
                    onChange={() => toggleId(id)}
                  />
                  <Label check htmlFor={`conn-${id}`}>
                    {getItemName(option)}
                  </Label>
                </FormGroup>
              );
            })}
          </div>
        )}
      </ModalBody>
      <ModalFooter>
        <Btn className="btn-outline-secondary" title="انصراف" onClick={toggle} />
        <Btn
          className="btn-primary"
          title="ذخیره"
          loading={isSaving}
          onClick={() => onSave?.(checked)}
        />
      </ModalFooter>
    </Modal>
  );
};

export default ConnectionsModal;
