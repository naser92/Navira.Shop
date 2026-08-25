using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Warehouse;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Mappers.ReadModel
{
    public class StockMovementModelMapping : EntityReadMapperBase<StockMovementModel, long>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<StockMovementModel> builder)
        {
            base.Configure(builder);

            builder.HasComment(";");

            builder.Property(t => t.ProductVariantId).IsRequired().HasComment("ProductVariantId");

            builder.Property(t => t.WarehouseId).IsRequired().HasComment("WarehouseId");

            builder.Property(t => t.MovementType).HasColumnType("varchar").HasMaxLength(30).IsRequired().HasComment("MovementType");

            builder.Property(t => t.Quantity).IsRequired().HasComment("Quantity");

            builder.Property(t => t.ReferenceType).HasColumnType("varchar").HasMaxLength(50).HasComment("ReferenceType");

            builder.Property(t => t.ReferenceId).HasComment("ReferenceId");

            builder.Property(t => t.Description).HasColumnType("nvarchar").HasMaxLength(500).HasComment("Description");

            builder.HasOne(x => x.ProductVariant).WithMany(x => x.StockMovement).HasForeignKey(x => x.ProductVariantId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Warehouse).WithMany(x => x.StockMovement).HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
