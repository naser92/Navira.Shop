using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Ecommerce;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class TaxRateMapping : EntityMapperBase<TaxRate, int>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<TaxRate> builder)
        {

            base.Configure(builder);

            builder.HasComment(";");
            builder.Property(t => t.TaxCategoryId).IsRequired().HasComment("TaxCategoryId");
            builder.Property(t => t.Percentage).HasColumnType("decimal").HasPrecision(5, 2).IsRequired().HasComment("Percentage");
            builder.Property(t => t.EffectiveFromUtc).IsRequired().HasComment("EffectiveFromUtc");
            builder.Property(t => t.EffectiveToUtc).HasComment("EffectiveToUtc");

            builder.HasOne(x => x.TaxCategory).WithMany(x => x.TaxRate).HasForeignKey(x => x.TaxCategoryId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
