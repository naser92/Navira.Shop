using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Media
{
    public class MediaVariant : FullEntity<long>, IFullAuditableEntity<Guid>
    {

        [ForeignKey("MediaFileId")]
        public virtual MediaFile MediaFile { get; set; }

        public Guid MediaFileId { get; set; }

        public string Profile { get; set; }

        public string Format { get; set; }

        public string ContentType { get; set; }

        public string Bucket { get; set; }

        public string ObjectKey { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public long SizeBytes { get; set; }

    }
}
