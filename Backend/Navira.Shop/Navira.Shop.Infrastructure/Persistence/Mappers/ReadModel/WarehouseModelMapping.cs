using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Warehouse;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Mappers.ReadModel
{
    public class WarehouseModelMapping : EntityReadMapperBase<WarehouseModel, int>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<WarehouseModel> builder)
        {
            base.Configure(builder);

            builder.HasComment(";");

            builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(200).IsRequired().HasComment("Name");

            builder.Property(t => t.Code).HasColumnType("varchar").HasMaxLength(50).IsRequired().HasComment("Code");

            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");

        }
    }
}
