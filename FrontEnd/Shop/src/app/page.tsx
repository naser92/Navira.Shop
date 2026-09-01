import { PageContainer } from "@/framework/ui/layout";
import styles from "./page.module.scss";

export default function HomePage() {
  return (
    <PageContainer as="main">
      <div className={styles.heroPlaceholder}>
        <h1>لحظه‌های گرم، انتخاب‌های ماندگار</h1>
        <p>فروشگاه تخصصی ماگ و هدیه‌های دوست‌داشتنی نویرا</p>
      </div>
    </PageContainer>
  );
}
