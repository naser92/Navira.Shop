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
            builder.Property(x => x.Name).HasColumnName("Name").HasColumnType("nvarchar(150)").ValueGeneratedNever().HasMaxLength(150).IsRequired();
            builder.Property(x => x.IsActive).HasColumnName("IsActive").HasColumnType("bit").ValueGeneratedNever().IsRequired();

            builder.Property(x => x.IsVariantAttribute).HasColumnName("IsVariantAttribute").HasColumnType("bit").ValueGeneratedNever().IsRequired();
            builder.Property(x => x.IsFilterable).HasColumnName("IsFilterable").HasColumnType("bit").ValueGeneratedNever().IsRequired();
            builder.Property(x => x.IsVisible).HasColumnName("IsVisible").HasColumnType("bit").ValueGeneratedNever().IsRequired();
            builder.Property(x => x.IsSearchable).HasColumnName("IsSearchable").HasColumnType("bit").ValueGeneratedNever().IsRequired();
            builder.Property(x => x.ValueType).HasColumnName("ValueType").HasColumnType("tinyint").ValueGeneratedNever().IsRequired();

            //builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(150).IsRequired().HasComment("Name");

            //builder.Property(t => t.ValueType).HasColumnType("varchar").HasMaxLength(30).IsRequired().HasComment("ValueType");

            //builder.Property(t => t.Usage).HasColumnType("varchar").HasMaxLength(30).IsRequired().HasComment("Usage");

            //builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");

        }
    }
}
