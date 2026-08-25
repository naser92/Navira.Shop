using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class BrandMapping : EntityMapperBase<Brand, int>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<Brand> builder)
        {

            base.Configure(builder);

            builder.HasComment(";");
            builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(200).IsRequired().HasComment("Name");
            builder.Property(t => t.Slug).HasColumnType("varchar").HasMaxLength(250).IsRequired().HasComment("Slug");
            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");


        }
    }
}
