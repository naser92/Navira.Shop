using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Warehouse;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Mappers.ReadModel
{
    public class StockModelMapping : EntityReadMapperBase<StockModel, long>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<StockModel> builder)
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
