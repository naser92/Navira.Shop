"use client";

import { useEffect, useMemo, useState } from "react";
import { FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import { RiSearchLine } from "react-icons/ri";
import Btn from "@/elements/buttons/Btn";
import { getItemId, getItemName } from "./accessUtils";

const searchableText = (option) =>
  [option?.name, option?.title, option?.code, option?.description]
    .filter((value) => value !== null && value !== undefined)
    .map(String)
    .join(" ")
    .toLowerCase();

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
  const [search, setSearch] = useState("");

  useEffect(() => {
    if (isOpen) {
      setChecked(selectedIds.map(String));
      setSearch("");
    }
  }, [isOpen, selectedIds]);

  const toggleId = (id) => {
    const key = String(id);
    setChecked((prev) =>
      prev.includes(key) ? prev.filter((item) => item !== key) : [...prev, key]
    );
  };

  const visibleOptions = useMemo(() => {
    const term = search.trim().toLowerCase();
    if (!term) return options;
    return options.filter((option) => searchableText(option).includes(term));
  }, [options, search]);

  return (
    <Modal isOpen={isOpen} toggle={toggle} centered className="navira-access-modal">
      <ModalHeader toggle={toggle}>{title}</ModalHeader>
      <ModalBody>
        {description && <p className="navira-access-modal-desc">{description}</p>}
        {options.length > 0 && (
          <div className="navira-table-search mb-3">
            <RiSearchLine size={17} />
            <Input
              type="text"
              value={search}
              onChange={(event) => setSearch(event.target.value)}
              placeholder="جستجو..."
              aria-label="جستجو"
            />
          </div>
        )}
        {options.length === 0 ? (
          <p className="navira-access-empty">موردی برای اتصال وجود ندارد.</p>
        ) : visibleOptions.length === 0 ? (
          <p className="navira-access-empty">موردی با این جستجو یافت نشد.</p>
        ) : (
          <div className="navira-access-options">
            {visibleOptions.map((option) => {
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
        <Btn className="btn-outline-secondary" title="انصراف" onClick={toggle} disabled={isSaving} />
        <Btn
          className="btn-primary"
          title="ذخیره"
          loading={isSaving}
          disabled={isSaving}
          onClick={() => onSave?.(checked)}
        />
      </ModalFooter>
    </Modal>
  );
};

export default ConnectionsModal;
