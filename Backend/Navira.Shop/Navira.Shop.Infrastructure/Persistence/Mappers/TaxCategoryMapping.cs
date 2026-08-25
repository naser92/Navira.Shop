using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Ecommerce;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class TaxCategoryMapping : EntityMapperBase<TaxCategory, int>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<TaxCategory> builder)
        {

            base.Configure(builder);

            builder.HasComment(";");
            builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(150).IsRequired().HasComment("Name");
            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");


        }
    }
}
