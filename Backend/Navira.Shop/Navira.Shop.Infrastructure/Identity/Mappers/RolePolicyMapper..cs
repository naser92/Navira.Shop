using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Infrastructure.Identity.Mappers.Model
{
    public class RolePolicyMapper : EntityMapperBase<RolePolicy, int>
    {
        public override void Configure(EntityTypeBuilder<RolePolicy> builder)
        {

            base.Configure(builder);

            builder.Property(t => t.RoleId).HasColumnType("nvarchar").HasMaxLength(100).IsRequired().HasComment("RoleId");
            builder.Property(t => t.PolicyId).IsRequired().HasComment("PolicyId");

            builder.HasOne(x => x.Policy).WithMany(x => x.RolePolicy).HasForeignKey(x => x.PolicyId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
