using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Mappers.ReadModel
{
    public class ProductVariantAttributeValueMapping : EntityReadMapperBase<ProductVariantAttributeValueModel, long>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<ProductVariantAttributeValueModel> builder)
        {
            base.Configure(builder);

            builder.HasComment(";");

            builder.Property(t => t.ProductVariantId).IsRequired().HasComment("ProductVariantId");

            builder.Property(t => t.ProductAttributeId).IsRequired().HasComment("ProductAttributeId");

            builder.Property(t => t.ProductAttributeOptionId).IsRequired().HasComment("ProductAttributeOptionId");

            builder.HasOne(x => x.ProductAttribute).WithMany(x => x.ProductVariantAttributeValue).HasForeignKey(x => x.ProductAttributeId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.ProductAttributeOption).WithMany(x => x.ProductVariantAttributeValue).HasForeignKey(x => x.ProductAttributeOptionId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.ProductVariant).WithMany(x => x.ProductVariantAttributeValue).HasForeignKey(x => x.ProductVariantId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
