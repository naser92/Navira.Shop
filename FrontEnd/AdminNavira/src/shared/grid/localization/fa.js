/** @typedef {import("../core/types.js").GridLocalization} GridLocalization */

import { formatNumber } from "../core/utils.js";

const OPERATOR_LABELS = {
  eq: "برابر",
  neq: "نابرابر",
  contains: "شامل",
  startsWith: "شروع با",
  endsWith: "پایان با",
  gt: "بزرگ‌تر از",
  gte: "بزرگ‌تر یا مساوی",
  lt: "کوچک‌تر از",
  lte: "کوچک‌تر یا مساوی",
  between: "بین",
  in: "یکی از",
  notIn: "هیچ‌کدام از",
  isNull: "خالی است",
  isNotNull: "خالی نیست",
};

/** @type {GridLocalization} */
export const fa = {
  searchPlaceholder: "جستجو...",
  searchLabel: "جستجو در نتایج",
  filters: "فیلترها",
  addFilter: "افزودن فیلتر",
  clearFilters: "حذف فیلترها",
  columns: "ستون‌ها",
  refresh: "به‌روزرسانی",
  retry: "تلاش مجدد",
  loading: "در حال بارگذاری...",
  errorLoading: "خطا در دریافت اطلاعات",
  noData: "اطلاعاتی برای نمایش وجود ندارد.",
  noFilteredData: "نتیجه‌ای مطابق جستجو یا فیلترهای انتخاب‌شده پیدا نشد.",
  previousPage: "قبلی",
  nextPage: "بعدی",
  rowsPerPage: "ردیف در صفحه",
  selectPage: "انتخاب همه ردیف‌های این صفحه",
  clearSelection: "لغو انتخاب",
  restoreDefaults: "بازگشت به پیش‌فرض",
  exportLabel: "خروجی",
  moveUp: "انتقال به بالا",
  moveDown: "انتقال به پایین",
  field: "فیلد",
  operator: "عملگر",
  value: "مقدار",
  from: "از",
  to: "تا",
  apply: "اعمال",
  remove: "حذف",
  selectedRows: (count) => `${formatNumber(count)} مورد انتخاب شده`,
  range: (from, to, total) =>
    `نمایش ${formatNumber(from)} تا ${formatNumber(to)} از ${formatNumber(total)} مورد`,
  sortAscLabel: (field) => `مرتب‌سازی صعودی بر اساس ${field}`,
  sortDescLabel: (field) => `مرتب‌سازی نزولی بر اساس ${field}`,
  operatorLabel: (op) => OPERATOR_LABELS[op] ?? op,
};
