using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class ProductAttributeOptionMapping : EntityMapperBase<ProductAttributeOption, int>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<ProductAttributeOption> builder)
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
