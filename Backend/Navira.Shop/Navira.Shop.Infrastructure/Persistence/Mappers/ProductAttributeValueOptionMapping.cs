using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class ProductAttributeValueOptionMapping : EntityMapperBase<ProductAttributeValueOption, long>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<ProductAttributeValueOption> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("bigint").ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.ProductAttributeValueId).HasColumnName("ProductAttributeValueId").HasColumnType("bigint").ValueGeneratedNever().IsRequired();
            builder.Property(x => x.ProductAttributeId).HasColumnName("ProductAttributeId").HasColumnType("int").ValueGeneratedNever().IsRequired();
            builder.Property(x => x.ProductAttributeOptionId).HasColumnName("ProductAttributeOptionId").HasColumnType("int").ValueGeneratedNever().IsRequired();

            builder.HasOne(x => x.ProductAttributeValue).WithMany(x => x.ProductAttributeValueOption).HasForeignKey(x => new { x.ProductAttributeValueId, x.ProductAttributeId }).HasPrincipalKey(x => new { x.Id, x.ProductAttributeId }).HasConstraintName("FK_ProductAttributeValueOption_Parent").OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.ProductAttributeOption).WithMany(x => x.ProductAttributeValueOption).HasForeignKey(x => new { x.ProductAttributeId, x.ProductAttributeOptionId }).HasPrincipalKey(x => new { x.ProductAttributeId, x.Id }).HasConstraintName("FK_ProductAttributeValueOption_AttributeOption").OnDelete(DeleteBehavior.NoAction);
        }
    }

}
