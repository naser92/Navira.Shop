using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Warehouse;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class StockMapping : EntityMapperBase<Stock, long>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<Stock> builder)
        {

            base.Configure(builder);

            builder.HasComment(";");
            builder.Property(t => t.ProductVariantId).IsRequired().HasComment("ProductVariantId");
            builder.Property(t => t.WarehouseId).IsRequired().HasComment("WarehouseId");
            builder.Property(t => t.QuantityOnHand).IsRequired().HasComment("QuantityOnHand");
            builder.Property(t => t.QuantityReserved).IsRequired().HasComment("QuantityReserved");

            builder.HasOne(x => x.ProductVariant).WithMany(x => x.Stock).HasForeignKey(x => x.ProductVariantId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Warehouse).WithMany(x => x.Stock).HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
