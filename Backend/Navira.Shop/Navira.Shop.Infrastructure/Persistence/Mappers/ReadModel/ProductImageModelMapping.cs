using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Mappers.ReadModel
{
    public class ProductImageModelMapping : EntityReadMapperBase<ProductImageModel, long>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<ProductImageModel> builder)
        {
            base.Configure(builder);

            builder.HasComment(";");

            builder.Property(t => t.ProductId).IsRequired().HasComment("ProductId");

            builder.Property(t => t.ProductVariantId).HasComment("ProductVariantId");

            builder.Property(t => t.Url).HasColumnType("nvarchar").HasMaxLength(1000).IsRequired().HasComment("Url");

            builder.Property(t => t.AltText).HasColumnType("nvarchar").HasMaxLength(300).HasComment("AltText");

            builder.Property(t => t.SortOrder).IsRequired().HasComment("SortOrder");

            builder.Property(t => t.IsPrimary).IsRequired().HasDefaultValue(true).HasComment("IsPrimary");

            builder.HasOne(x => x.Product).WithMany(x => x.ProductImage).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.ProductVariant).WithMany(x => x.ProductImage).HasForeignKey(x => x.ProductVariantId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
