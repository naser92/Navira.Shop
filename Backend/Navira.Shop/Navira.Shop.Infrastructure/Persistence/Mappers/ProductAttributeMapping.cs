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
            builder.Property(x => x.ValueType).HasConversion<byte>().HasColumnType("tinyint").IsRequired();
            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");
            builder.Property(t => t.IsVariantAttribute).IsRequired().HasDefaultValue(false).HasComment("IsVariantAttribute ");
            builder.Property(t => t.IsFilterable).IsRequired().HasDefaultValue(false).HasComment("IsFilterable");
            builder.Property(t => t.IsVisible).IsRequired().HasDefaultValue(true).HasComment("IsVisible");
            builder.Property(t => t.IsSearchable).IsRequired().HasDefaultValue(true).HasComment("IsSearchable");


        }
    }
}
