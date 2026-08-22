"use client";

import { useState } from "react";
import { RiCheckLine, RiAddLine } from "react-icons/ri";
import { Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import Btn from "@/elements/buttons/Btn";
import { useAccessCreate } from "@/utils/hooks/access/useAccessCrud";

/**
 * Create policy request matching backend PolicyRegisterCommand (POST /api/Policy).
 * @typedef {Object} CreatePolicyRequest
 * @property {string|null} name
 * @property {string|null} title
 * @property {string|null} description
 * @property {boolean} isSystem
 * @property {boolean} isActive
 */

const formTexts = {
  Add: "افزودن سیاست",
  Title: "ایجاد سیاست جدید",
  Name: "نام سیاست",
  NamePlaceholder: "مثلاً: OperationalAccess",
  PolicyTitle: "عنوان",
  TitlePlaceholder: "مثلاً: دسترسی عملیاتی",
  Description: "توضیحات",
  DescriptionPlaceholder: "توضیح کوتاه درباره این سیاست...",
  IsSystem: "سیستمی",
  IsActive: "فعال",
  Cancel: "انصراف",
  Submit: "ایجاد سیاست",
};

const INITIAL_FORM = {
  name: "",
  title: "",
  description: "",
  isSystem: true,
  isActive: true,
};

const toNullable = (value) => (value.trim() === "" ? null : value.trim());

const PolicyFormModal = ({ onCreated }) => {
  const [isOpen, setIsOpen] = useState(false);
  const [form, setForm] = useState(INITIAL_FORM);
  const create = useAccessCreate("policies");

  const toggle = () => {
    if (create.isPending) return;
    setIsOpen((prev) => !prev);
  };

  const reset = () => {
    setForm(INITIAL_FORM);
  };

  const setField = (field) => (event) => {
    const value = event.target.type === "checkbox" ? event.target.checked : event.target.value;
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    /** @type {CreatePolicyRequest} */
    const body = {
      name: toNullable(form.name),
      title: toNullable(form.title),
      description: toNullable(form.description),
      isSystem: form.isSystem,
      isActive: form.isActive,
    };

    create.mutate(body, {
      onSuccess: () => {
        toggle();
        reset();
        onCreated?.();
      },
    });
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
                value={form.name}
                onChange={setField("name")}
                placeholder={formTexts.NamePlaceholder}
                autoFocus
              />
            </FormGroup>
            <FormGroup>
              <Label for="policy-title">{formTexts.PolicyTitle}</Label>
              <Input
                id="policy-title"
                value={form.title}
                onChange={setField("title")}
                placeholder={formTexts.TitlePlaceholder}
              />
            </FormGroup>
            <FormGroup>
              <Label for="policy-description">{formTexts.Description}</Label>
              <Input
                id="policy-description"
                type="textarea"
                rows={3}
                value={form.description}
                onChange={setField("description")}
                placeholder={formTexts.DescriptionPlaceholder}
              />
            </FormGroup>
            <FormGroup check className="mb-2">
              <Input
                id="policy-is-system"
                type="checkbox"
                checked={form.isSystem}
                onChange={setField("isSystem")}
              />
              <Label for="policy-is-system" check>
                {formTexts.IsSystem}
              </Label>
            </FormGroup>
            <FormGroup check className="mb-0">
              <Input
                id="policy-is-active"
                type="checkbox"
                checked={form.isActive}
                onChange={setField("isActive")}
              />
              <Label for="policy-is-active" check>
                {formTexts.IsActive}
              </Label>
            </FormGroup>
          </ModalBody>
          <ModalFooter>
            <Btn
              className="btn-outline-secondary"
              title={formTexts.Cancel}
              onClick={toggle}
              disabled={create.isPending}
            />
            <Btn
              className="btn-primary"
              type="submit"
              title={formTexts.Submit}
              loading={create.isPending}
              disabled={create.isPending}
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
