import { useState } from "react";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import { StringFilter } from "../filters/StringFilter.js";
import { NumberFilter } from "../filters/NumberFilter.js";
import { DateFilter } from "../filters/DateFilter.js";
import { BooleanFilter } from "../filters/BooleanFilter.js";
import { SelectFilter } from "../filters/SelectFilter.js";
import { operatorsForColumn } from "../core/operators.js";

const FILTER_COMPONENTS = {
  string: StringFilter,
  number: NumberFilter,
  date: DateFilter,
  datetime: DateFilter,
  boolean: BooleanFilter,
  enum: SelectFilter,
  select: SelectFilter,
};

export function GridFilters({ columns, filters, setFilters, localization }) {
  const [isOpen, setIsOpen] = useState(false);
  const [newFilter, setNewFilter] = useState(null);

  const toggle = () => setIsOpen(!isOpen);

  const addFilter = () => {
    if (newFilter?.field && newFilter?.operator) {
      setFilters([...filters, newFilter]);
      setNewFilter(null);
      toggle();
    }
  };

  const updateFilter = (index, key, value) => {
    const updated = [...filters];
    updated[index] = { ...updated[index], [key]: value };
    setFilters(updated);
  };

  const removeFilter = (index) => {
    setFilters(filters.filter((_, i) => i !== index));
  };

  const clearFilters = () => {
    setFilters([]);
  };

  const filterableColumns = columns.filter(c => c.filterable !== false);

  return (
    <>
      <Button size="sm" color="outline-secondary" onClick={toggle}>
        {localization.filters} ({filters.length})
      </Button>

      <Modal isOpen={isOpen} toggle={toggle}>
        <ModalHeader toggle={toggle}>{localization.filters}</ModalHeader>
        <ModalBody>
          {filters.map((filter, index) => {
            const column = columns.find(c => c.field === filter.field);
            if (!column) return null;

            const FilterComponent = FILTER_COMPONENTS[column.type] || StringFilter;
            const operators = operatorsForColumn(column);

            return (
              <div key={index} className="navira-grid-filter-row">
                <div className="navira-grid-filter-field">
                  <label>{column.title}</label>
                </div>
                
                <div className="navira-grid-filter-operator">
                  <select
                    value={filter.operator}
                    onChange={(e) => updateFilter(index, 'operator', e.target.value)}
                  >
                    {operators.map(op => (
                      <option key={op} value={op}>
                        {localization.operatorLabel(op)}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="navira-grid-filter-value">
                  <FilterComponent
                    value={filter.value}
                    onChange={(val) => updateFilter(index, 'value', val)}
                    options={column.filter?.options}
                  />
                </div>

                <Button size="sm" color="danger" onClick={() => removeFilter(index)}>
                  {localization.remove}
                </Button>
              </div>
            );
          })}

          {newFilter && (
            <div className="navira-grid-filter-row navira-grid-new-filter">
              <select
                value={newFilter.field || ''}
                onChange={(e) => setNewFilter({ ...newFilter, field: e.target.value })}
              >
                <option value="">{localization.field}</option>
                {filterableColumns.map(col => (
                  <option key={col.field} value={col.field}>
                    {col.title}
                  </option>
                ))}
              </select>
              
              {newFilter.field && (
                <select
                  value={newFilter.operator || ''}
                  onChange={(e) => setNewFilter({ ...newFilter, operator: e.target.value })}
                >
                  <option value="">{localization.operator}</option>
                  {operatorsForColumn(
                    filterableColumns.find(c => c.field === newFilter.field)
                  ).map(op => (
                    <option key={op} value={op}>
                      {localization.operatorLabel(op)}
                    </option>
                  ))}
                </select>
              )}
            </div>
          )}

          <Button size="sm" color="secondary" onClick={() => setNewFilter({})}>
            + {localization.addFilter}
          </Button>

          {filters.length > 0 && (
            <Button size="sm" color="link" onClick={clearFilters}>
              {localization.clearFilters}
            </Button>
          )}
        </ModalBody>
        <ModalFooter>
          <Button color="secondary" onClick={toggle}>
            {localization.apply}
          </Button>
        </ModalFooter>
      </Modal>
    </>
  );
}
