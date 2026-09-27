using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Warehouse;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class StockMovementMapping : EntityMapperBase<StockMovement, long>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<StockMovement> builder)
        {

            base.Configure(builder);

            builder.Property(t => t.ProductVariantId).IsRequired().HasComment("ProductVariantId");
            builder.Property(t => t.WarehouseId).IsRequired().HasComment("WarehouseId");
            builder.Property(t => t.MovementType).IsRequired().HasComment("MovementType");
            builder.Property(t => t.Quantity).IsRequired().HasComment("Quantity");
            builder.Property(t => t.ReferenceType).HasColumnType("varchar").HasMaxLength(50).HasComment("ReferenceType");
            builder.Property(t => t.ReferenceId).HasComment("ReferenceId");
            builder.Property(t => t.Description).HasColumnType("nvarchar").HasMaxLength(500).HasComment("Description");
            builder.Property(t => t.OperationId).IsRequired().HasComment("OperationId");

            builder.HasOne(x => x.ProductVariant).WithMany(x => x.StockMovement).HasForeignKey(x => x.ProductVariantId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Warehouse).WithMany(x => x.StockMovement).HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
