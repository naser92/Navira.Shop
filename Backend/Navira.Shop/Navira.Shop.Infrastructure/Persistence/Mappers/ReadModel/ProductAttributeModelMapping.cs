using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Mappers.ReadModel
{
    public class ProductAttributeModelMapping : EntityReadMapperBase<ProductAttributeModel, int>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<ProductAttributeModel> builder)
        {
            base.Configure(builder);

            builder.HasComment(";");

            builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(150).IsRequired().HasComment("Name");

            builder.Property(t => t.ValueType).HasColumnType("varchar").HasMaxLength(30).IsRequired().HasComment("ValueType");

            builder.Property(t => t.Usage).HasColumnType("varchar").HasMaxLength(30).IsRequired().HasComment("Usage");

            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");

        }
    }
}
