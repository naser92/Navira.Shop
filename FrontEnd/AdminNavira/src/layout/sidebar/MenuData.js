import { RiShieldKeyholeLine } from "react-icons/ri";

const MENUITEMS = [
  {
    title: "احراز هویت",
    icon: <RiShieldKeyholeLine />,
    type: "sub",
    children: [
      { title: "دسترسی‌ها", path: "/access" },
    ],
  },
];

export default MENUITEMS;
