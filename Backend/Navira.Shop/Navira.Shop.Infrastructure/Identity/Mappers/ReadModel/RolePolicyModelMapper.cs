using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Identity.Mappers.ReadModel
{
    public class RolePolicyModelMapper : EntityReadMapperBase<RolePolicyModel, int>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<RolePolicyModel> builder)
        {
            base.Configure(builder);


            builder.Property(t => t.RoleId).HasColumnType("nvarchar").HasMaxLength(100).IsRequired().HasComment("RoleId");

            builder.Property(t => t.PolicyId).IsRequired().HasComment("PolicyId");

            builder.HasOne(x => x.Policy).WithMany(x => x.RolePolicy).HasForeignKey(x => x.PolicyId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
