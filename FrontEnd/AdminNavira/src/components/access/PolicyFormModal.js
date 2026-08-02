"use client";

import { useState } from "react";
import { RiCheckLine, RiAddLine } from "react-icons/ri";
import { Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import Btn from "@/elements/buttons/Btn";
import { useAccessCreate } from "@/utils/hooks/access/useAccessCrud";

const formTexts = {
  Add: "افزودن سیاست",
  Title: "ایجاد سیاست جدید",
  Name: "نام سیاست",
  NamePlaceholder: "مثلاً: دسترسی عملیاتی",
  Description: "توضیحات",
  DescriptionPlaceholder: "توضیح کوتاه درباره این سیاست...",
  Cancel: "انصراف",
  Submit: "ایجاد سیاست",
  NameRequired: "نام سیاست الزامی است",
};

const PolicyFormModal = ({ onCreated }) => {
  const [isOpen, setIsOpen] = useState(false);
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [error, setError] = useState("");
  const create = useAccessCreate("policies");

  const toggle = () => {
    setIsOpen((prev) => !prev);
    setError("");
  };

  const reset = () => {
    setName("");
    setDescription("");
    setError("");
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    if (!name.trim()) {
      setError(formTexts.NameRequired);
      return;
    }
    create.mutate(
      { name: name.trim(), description: description.trim(), permissions: [] },
      {
        onSuccess: () => {
          toggle();
          reset();
          onCreated?.();
        },
      }
    );
  };

  return (
    <>
      <button type="button" className="navira-action-btn" onClick={toggle}>
        <RiAddLine size={16} />
        <span>{formTexts.Add}</span>
      </button>

      <Modal isOpen={isOpen} toggle={toggle} centered className="navira-access-modal">
        <ModalHeader toggle={toggle}>{formTexts.Title}</ModalHeader>
        <Form onSubmit={handleSubmit}>
          <ModalBody>
            <FormGroup>
              <Label for="policy-name">{formTexts.Name}</Label>
              <Input
                id="policy-name"
                value={name}
                onChange={(event) => {
                  setName(event.target.value);
                  if (error) setError("");
                }}
                placeholder={formTexts.NamePlaceholder}
                invalid={Boolean(error)}
                autoFocus
              />
              {error && <div className="invalid-feedback d-block">{error}</div>}
            </FormGroup>
            <FormGroup className="mb-0">
              <Label for="policy-description">{formTexts.Description}</Label>
              <Input
                id="policy-description"
                type="textarea"
                rows={3}
                value={description}
                onChange={(event) => setDescription(event.target.value)}
                placeholder={formTexts.DescriptionPlaceholder}
              />
            </FormGroup>
          </ModalBody>
          <ModalFooter>
            <Btn className="btn-outline-secondary" title={formTexts.Cancel} onClick={toggle} />
            <Btn
              className="btn-primary"
              type="submit"
              title={formTexts.Submit}
              loading={create.isPending}
            >
              <RiCheckLine size={16} />
            </Btn>
          </ModalFooter>
        </Form>
      </Modal>
    </>
  );
};

export default PolicyFormModal;
