using Microsoft.AspNetCore.Http;
using Navira.Shop.Domain.Media;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Media
{
    public sealed class UploadMediaRequest
    {
        [Required]
        public IFormFile File { get; set; } = default!;

        [Required]
        [EnumDataType(typeof(MediaUploadProfile))]
        public MediaUploadProfile? Profile { get; set; }

        [Required]
        public ImageEditDto Edit { get; set; } = new();
    }
}
