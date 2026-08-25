using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Ecommerce;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Mappers.ReadModel
{
    public class TaxCategoryModelMapping : EntityReadMapperBase<TaxCategoryModel, int>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<TaxCategoryModel> builder)
        {
            base.Configure(builder);

            builder.HasComment(";");

            builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(150).IsRequired().HasComment("Name");

            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");

        }
    }
}
