using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Warehouse;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class WarehouseMapping : EntityMapperBase<Warehouse, int>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<Warehouse> builder)
        {

            base.Configure(builder);

            builder.HasComment(";");
            builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(200).IsRequired().HasComment("Name");
            builder.Property(t => t.Code).HasColumnType("varchar").HasMaxLength(50).IsRequired().HasComment("Code");
            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");


        }
    }
}
