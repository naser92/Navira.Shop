import styles from "./HeroProductSlider.module.scss";

export function HeroModelText({ value }: { value: string }) {
  return (
    <div className={styles.modelText} aria-hidden="true">
      {Array.from(value).map((letter, index) => (
        <span key={`${letter}-${index}`} data-model-letter>
          {letter === " " ? "\u00a0" : letter}
        </span>
      ))}
    </div>
  );
}

