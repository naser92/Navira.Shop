using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;

namespace Navira.Shop.Application.Media
{
    public class MediaFileModel : BaseReadModel<Guid>, IAuditableEntity
    {

        public string ProcessingProfile { get; set; }

        public int ProcessingProfileVersion { get; set; }

        public string EditSettingsJson { get; set; }

        public string OriginalFileName { get; set; }

        public string ContentType { get; set; }

        public long SizeBytes { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string StorageProvider { get; set; }

        public string Bucket { get; set; }

        public string ObjectKey { get; set; }

        public int Status { get; set; }

        public DateTime? ExpiresAt { get; set; }
        public virtual ICollection<MediaVariantModel> MediaVariant { get; set; }

    }
}
