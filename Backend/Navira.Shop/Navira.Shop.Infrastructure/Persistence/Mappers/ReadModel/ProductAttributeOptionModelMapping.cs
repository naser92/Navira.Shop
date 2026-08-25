using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Mappers.ReadModel
{
    public class ProductAttributeOptionModelMapping : EntityReadMapperBase<ProductAttributeOptionModel, int>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<ProductAttributeOptionModel> builder)
        {
            base.Configure(builder);

            builder.HasComment(";");

            builder.Property(t => t.ProductAttributeId).IsRequired().HasComment("ProductAttributeId");

            builder.Property(t => t.Value).HasColumnType("nvarchar").HasMaxLength(200).IsRequired().HasComment("Value");

            builder.Property(t => t.SortOrder).IsRequired().HasComment("SortOrder");

            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");

            builder.HasOne(x => x.ProductAttribute).WithMany(x => x.ProductAttributeOption).HasForeignKey(x => x.ProductAttributeId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
