using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Mappers.ReadModel
{
    public class BrandModelMapping : EntityReadMapperBase<BrandModel, int>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<BrandModel> builder)
        {
            base.Configure(builder);

            builder.HasComment(";");

            builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(200).IsRequired().HasComment("Name");

            builder.Property(t => t.Slug).HasColumnType("varchar").HasMaxLength(250).IsRequired().HasComment("Slug");

            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");

        }
    }
}
