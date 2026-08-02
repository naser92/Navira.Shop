using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Infrastructure.Identity.Mappers.Model
{
    public class PolicyPermissionMapper : EntityMapperBase<PolicyPermission, int>
    {
        public override void Configure(EntityTypeBuilder<PolicyPermission> builder)
        {

            base.Configure(builder);


            builder.HasOne(x => x.Permission).WithMany(x => x.PolicyPermission).HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Policy).WithMany(x => x.PolicyPermission).HasForeignKey(x => x.PolicyId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
