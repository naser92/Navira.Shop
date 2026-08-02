using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Identity.Mappers.ReadModel
{
    public class PolicyModelMapper : EntityReadMapperBase<PolicyModel, int>
    {

        public override void Configure(EntityTypeBuilder<PolicyModel> builder)
        {
            base.Configure(builder);


            builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(100).IsRequired().HasComment("Name");

            builder.Property(t => t.Title).HasColumnType("nvarchar").HasMaxLength(200).IsRequired().HasComment("Title");

            builder.Property(t => t.Description).HasColumnType("nvarchar").HasMaxLength(500).HasComment("Description");

            builder.Property(t => t.IsSystem).IsRequired().HasDefaultValue(true).HasComment("IsSystem");

            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");

        }
    }
}
