import {
  RiArticleLine,
  RiCoupon2Line,
  RiHomeLine,
  RiImageLine,
  RiListUnordered,
  RiPercentLine,
  RiQuestionnaireLine,
  RiSettings3Line,
  RiStore2Line,
  RiStore3Line,
  RiContactsLine,
  RiTruckLine,
  RiWalletLine,
} from "react-icons/ri";

const MENUITEMS = [
  {
    title: "داشبورد",
    icon: <RiHomeLine />,
    path: "/dashboard",
    type: "link",
  },
  {
    title: "کاربران",
    icon: <RiContactsLine />,
    type: "sub",
    children: [
      { title: "افزودن کاربر", path: "/user/create" },
      { title: "همه کاربران", path: "/user" },
      { title: "نقش‌ها", path: "/role" },
    ],
  },
  {
    title: "محصولات",
    icon: <RiStore3Line />,
    type: "sub",
    children: [
      { title: "افزودن محصول", path: "/product/create" },
      { title: "همه محصولات", path: "/product" },
      { title: "ویژگی‌ها", path: "/attribute" },
      { title: "دسته‌بندی‌ها", path: "/category" },
      { title: "برچسب‌ها", path: "/tag" },
      { title: "برندها", path: "/brand" },
    ],
  },
  {
    title: "فروشگاه‌ها",
    icon: <RiStore2Line />,
    type: "sub",
    children: [
      { title: "افزودن فروشگاه", path: "/store/create" },
      { title: "همه فروشگاه‌ها", path: "/store" },
      { title: "کیف پول", path: "/vendor_wallet" },
      { title: "تاریخچه کمیسیون", path: "/commission" },
      { title: "درخواست برداشت", path: "/withdraw_request" },
    ],
  },
  {
    title: "سفارش‌ها",
    icon: <RiListUnordered />,
    type: "sub",
    children: [
      { title: "همه سفارش‌ها", path: "/order" },
      { title: "ایجاد سفارش", path: "/order/create" },
    ],
  },
  {
    title: "رسانه",
    icon: <RiImageLine />,
    path: "/attachment",
    type: "link",
  },
  {
    title: "وبلاگ",
    icon: <RiArticleLine />,
    type: "sub",
    children: [
      { title: "افزودن پست", path: "/blog/create" },
      { title: "همه پست‌ها", path: "/blog" },
    ],
  },
  {
    title: "بازاریابی",
    icon: <RiPercentLine />,
    type: "sub",
    children: [
      { title: "کوپن‌ها", path: "/coupon" },
      { title: "مالیات", path: "/tax" },
      { title: "امتیازها", path: "/point" },
    ],
  },
  {
    title: "حمل و نقل",
    icon: <RiTruckLine />,
    path: "/shipping",
    type: "link",
  },
  {
    title: "سوالات متداول",
    icon: <RiQuestionnaireLine />,
    path: "/faq",
    type: "link",
  },
  {
    title: "مالی",
    icon: <RiWalletLine />,
    type: "sub",
    children: [
      { title: "کیف پول", path: "/wallet" },
      { title: "تراکنش‌ها", path: "/transaction" },
    ],
  },
  {
    title: "تنظیمات",
    icon: <RiSettings3Line />,
    path: "/setting",
    type: "link",
  },
];

export default MENUITEMS;
