using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Identity.Mappers.ReadModel
{
    public class MenuModelMapper : EntityReadMapperBase<MenuModel, int>
    {

        public override void Configure(EntityTypeBuilder<MenuModel> builder)
        {
            base.Configure(builder);
            builder.ToTable("Menu");

            builder.Property(t => t.ParentId).HasComment("ParentId");

            builder.Property(t => t.PermissionId).IsRequired().HasComment("PermissionId");

            builder.Property(t => t.Title).HasColumnType("nvarchar").HasMaxLength(200).IsRequired().HasComment("Title");

            builder.Property(t => t.Route).HasColumnType("nvarchar").HasMaxLength(300).IsRequired().HasComment("Route");

            builder.Property(t => t.Icon).HasColumnType("nvarchar").HasMaxLength(100).HasComment("Icon");

            builder.Property(t => t.SortOrder).IsRequired().HasComment("SortOrder");

            builder.Property(t => t.IsVisible).IsRequired().HasDefaultValue(true).HasComment("IsVisible");

            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");

            builder.HasOne(x => x.Parent).WithMany(x => x.Childs).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Permission).WithMany(x => x.Menu).HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
