using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Media;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class MediaVariantMapping : EntityMapperBase<MediaVariant, long>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<MediaVariant> builder)
        {

            base.Configure(builder);

            builder.HasComment(";");
            builder.Property(t => t.MediaFileId).IsRequired().HasComment("MediaFileId");
            builder.Property(t => t.Profile).HasColumnType("varchar").HasMaxLength(64).IsRequired().HasComment("Profile");
            builder.Property(t => t.Format).HasColumnType("varchar").HasMaxLength(10).IsRequired().HasComment("Format");
            builder.Property(t => t.ContentType).HasColumnType("varchar").HasMaxLength(100).IsRequired().HasComment("ContentType");
            builder.Property(t => t.Bucket).HasColumnType("varchar").HasMaxLength(63).IsRequired().HasComment("Bucket");
            builder.Property(t => t.ObjectKey).HasColumnType("nvarchar").HasMaxLength(1024).IsRequired().HasComment("ObjectKey");
            builder.Property(t => t.Width).IsRequired().HasComment("Width");
            builder.Property(t => t.Height).IsRequired().HasComment("Height");
            builder.Property(t => t.SizeBytes).IsRequired().HasComment("SizeBytes");

            builder.HasOne(x => x.MediaFile).WithMany(x => x.MediaVariant).HasForeignKey(x => x.MediaFileId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
