using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class ProductAttributeValueMapping : EntityMapperBase<ProductAttributeValue, long>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<ProductAttributeValue> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.ProductId).HasColumnName("ProductId").HasColumnType("int").ValueGeneratedNever().IsRequired();
            builder.Property(x => x.ProductAttributeId).HasColumnName("ProductAttributeId").HasColumnType("int").ValueGeneratedNever().IsRequired();
            builder.Property(x => x.ProductAttributeOptionId).HasColumnName("ProductAttributeOptionId").HasColumnType("int").ValueGeneratedNever().IsRequired(false);
            builder.Property(x => x.TextValue).HasColumnName("TextValue").HasColumnType("nvarchar(2000)").ValueGeneratedNever().HasMaxLength(2000).IsRequired(false);
            builder.Property(x => x.NumberValue).HasColumnName("NumberValue").HasColumnType("decimal(18,4)").ValueGeneratedNever().HasPrecision(18, 4).IsRequired(false);
            builder.Property(x => x.BooleanValue).HasColumnName("BooleanValue").HasColumnType("bit").ValueGeneratedNever().IsRequired(false);
            builder.Property(x => x.DateValue).HasColumnName("DateValue").HasColumnType("date").ValueGeneratedNever().IsRequired(false);

            builder.HasOne(x => x.Product).WithMany(x => x.ProductAttributeValue).HasForeignKey(x => x.ProductId).HasPrincipalKey(x => x.Id).HasConstraintName("FK_ProductAttributeValue_Product").OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.ProductAttribute).WithMany(x => x.ProductAttributeValue).HasForeignKey(x => x.ProductAttributeId).HasPrincipalKey(x => x.Id).HasConstraintName("FK_ProductAttributeValue_Attribute").OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.ProductAttributeOption).WithMany(x => x.ProductAttributeValue).HasForeignKey(x => new { x.ProductAttributeId, x.ProductAttributeOptionId }).HasPrincipalKey(x => new { x.ProductAttributeId, x.Id }).HasConstraintName("FK_ProductAttributeValue_AttributeOption").OnDelete(DeleteBehavior.NoAction);
        }
    }
}
