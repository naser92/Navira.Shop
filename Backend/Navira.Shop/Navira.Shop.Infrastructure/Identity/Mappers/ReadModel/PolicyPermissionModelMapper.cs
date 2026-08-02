using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Identity.Mappers.ReadModel
{
    public class PolicyPermissionModelMapper : EntityReadMapperBase<PolicyPermissionModel, int>
    {

        public override void Configure(EntityTypeBuilder<PolicyPermissionModel> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Permission).WithMany(x => x.PolicyPermission).HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Policy).WithMany(x => x.PolicyPermission).HasForeignKey(x => x.PolicyId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
