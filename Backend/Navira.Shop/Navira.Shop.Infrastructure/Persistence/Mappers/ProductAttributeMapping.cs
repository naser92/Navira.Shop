using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class ProductAttributeMapping : EntityMapperBase<ProductAttribute, int>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<ProductAttribute> builder)
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
