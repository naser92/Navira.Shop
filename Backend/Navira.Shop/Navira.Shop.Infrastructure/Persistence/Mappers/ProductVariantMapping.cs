using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class ProductVariantMapping : EntityMapperBase<ProductVariant, int>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<ProductVariant> builder)
        {

            base.Configure(builder);

            builder.HasComment(";");
            builder.Property(t => t.ProductId).IsRequired().HasComment("ProductId");
            builder.Property(t => t.Sku).HasColumnType("varchar").HasMaxLength(100).IsRequired().HasComment("Sku");
            builder.Property(t => t.Price).HasColumnType("decimal").HasPrecision(18, 2).IsRequired().HasComment("Price");
            builder.Property(t => t.CostPrice).HasColumnType("decimal").HasPrecision(18, 2).HasComment("CostPrice");
            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");

            builder.HasOne(x => x.Product).WithMany(x => x.ProductVariant).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
