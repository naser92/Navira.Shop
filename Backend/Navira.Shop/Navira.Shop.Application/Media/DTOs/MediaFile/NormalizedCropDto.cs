using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Media
{
    public sealed class NormalizedCropDto : IValidatableObject
    {
        [Range(0.0, 1.0)]
        public double X { get; set; }

        [Range(0.0, 1.0)]
        public double Y { get; set; }

        [Range(0.000001, 1.0)]
        public double Width { get; set; } = 1;

        [Range(0.000001, 1.0)]
        public double Height { get; set; } = 1;

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (!double.IsFinite(X) ||
                !double.IsFinite(Y) ||
                !double.IsFinite(Width) ||
                !double.IsFinite(Height))
            {
                yield return new ValidationResult(
                    "مختصات برش باید اعداد معتبر باشند.");

                yield break;
            }

            const double tolerance = 0.000000001;

            if (X + Width > 1 + tolerance)
            {
                yield return new ValidationResult(
                    "کادر برش از عرض تصویر خارج شده است.",
                    new[] { nameof(X), nameof(Width) });
            }

            if (Y + Height > 1 + tolerance)
            {
                yield return new ValidationResult(
                    "کادر برش از ارتفاع تصویر خارج شده است.",
                    new[] { nameof(Y), nameof(Height) });
            }
        }
    }
}
