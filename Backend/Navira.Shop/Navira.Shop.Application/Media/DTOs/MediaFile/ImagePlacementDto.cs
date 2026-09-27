using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Media
{
    public sealed class ImagePlacementDto
    {
        // مرکز تصویر روی بوم خروجی
        [Range(0.0, 1.0)]
        public double CenterX { get; set; } = 0.5;

        [Range(0.0, 1.0)]
        public double CenterY { get; set; } = 0.5;

        // اندازه نسبت به بزرگ‌ترین حالت جاگیری بدون برش
        [Range(0.01, 1.0)]
        public double Scale { get; set; } = 1;
    }
}
