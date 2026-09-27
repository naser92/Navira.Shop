using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Media
{
    public sealed class ImageEditDto
    {
        [Required]
        public NormalizedCropDto Crop { get; set; } = new();

        [Required]
        public ImagePlacementDto Placement { get; set; } = new();

        // فقط رنگ مات در نسخه اول
        [Required]
        [RegularExpression(
            "^#[0-9a-fA-F]{6}$",
            ErrorMessage = "رنگ پس‌زمینه باید مانند #FFFFFF باشد.")]
        public string BackgroundColor { get; set; } = "#FFFFFF";
    }
}
