using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class RolePolicyMapper : EntityMapperBase<RolePolicy, int>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<RolePolicy> builder)
        {

            base.Configure(builder);

            builder.Property(t => t.RoleId).IsRequired().HasComment("RoleId");
            builder.Property(t => t.PolicyId).IsRequired().HasComment("PolicyId");

            builder.HasOne(x => x.Policy).WithMany(x => x.RolePolicy).HasForeignKey(x => x.PolicyId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
