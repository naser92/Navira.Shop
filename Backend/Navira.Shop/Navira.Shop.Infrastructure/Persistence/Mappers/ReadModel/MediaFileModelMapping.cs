using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Media;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Mappers.ReadModel
{
    public class MediaFileMapping : EntityReadMapperBase<MediaFileModel, Guid>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<MediaFileModel> builder)
        {
            base.Configure(builder);

            builder.HasComment(";");

            builder.Property(t => t.ProcessingProfile).HasColumnType("varchar").HasMaxLength(50).IsRequired().HasComment("ProcessingProfile");

            builder.Property(t => t.ProcessingProfileVersion).IsRequired().HasComment("ProcessingProfileVersion");

            builder.Property(t => t.EditSettingsJson).HasColumnType("nvarcharmax").HasMaxLength(-1).IsRequired().HasComment("EditSettingsJson");

            builder.Property(t => t.OriginalFileName).HasColumnType("nvarchar").HasMaxLength(255).IsRequired().HasComment("OriginalFileName");

            builder.Property(t => t.ContentType).HasColumnType("varchar").HasMaxLength(100).IsRequired().HasComment("ContentType");

            builder.Property(t => t.SizeBytes).IsRequired().HasComment("SizeBytes");

            builder.Property(t => t.Width).IsRequired().HasComment("Width");

            builder.Property(t => t.Height).IsRequired().HasComment("Height");

            builder.Property(t => t.StorageProvider).HasColumnType("varchar").HasMaxLength(30).IsRequired().HasComment("StorageProvider");

            builder.Property(t => t.Bucket).HasColumnType("varchar").HasMaxLength(63).HasComment("Bucket");

            builder.Property(t => t.ObjectKey).HasColumnType("nvarchar").HasMaxLength(1024).HasComment("ObjectKey");

            builder.Property(t => t.Status).IsRequired().HasComment("Status");

            builder.Property(t => t.ExpiresAt).HasComment("ExpiresAt");


        }
    }
}
