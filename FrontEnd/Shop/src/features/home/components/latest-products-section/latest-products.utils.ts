export const formatProductPrice = (value: number) => `${new Intl.NumberFormat("fa-IR").format(value)} تومان`;
export const getDiscountPercent = (price: number, oldPrice?: number) => oldPrice && oldPrice > price ? Math.round((1 - price / oldPrice) * 100) : null;

