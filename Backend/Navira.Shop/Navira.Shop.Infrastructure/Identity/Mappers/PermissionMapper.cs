using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Infrastructure.Identity.Mappers.Model
{
    public class PermissionMapper : EntityMapperBase<Permission, int>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {

            base.Configure(builder);
            builder.Property(t => t.BaseSubSystemId).IsRequired().HasComment("BaseSubSystemId");
            builder.Property(t => t.ControllerName).HasColumnType("nvarchar").HasMaxLength(100).IsRequired().HasComment("ControllerName");
            builder.Property(t => t.Scope).HasColumnType("nvarchar").HasMaxLength(50).IsRequired().HasComment("Scope");
            builder.Property(t => t.Code).HasColumnType("nvarchar").HasMaxLength(151).IsRequired().HasComment("Code");
            builder.Property(t => t.Title).HasColumnType("nvarchar").HasMaxLength(200).IsRequired().HasComment("Title");
            builder.Property(t => t.Description).HasColumnType("nvarchar").HasMaxLength(500).HasComment("Description");
            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");


        }
    }
}
