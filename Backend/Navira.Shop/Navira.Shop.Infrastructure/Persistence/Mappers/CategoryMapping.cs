using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Persistence.Mappers
{
    public class CategoryMapping : EntityMapperBase<Category, int>, IWriteEntityConfiguration
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {

            base.Configure(builder);

            builder.HasComment(";");
            builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(200).IsRequired().HasComment("Name");
            builder.Property(t => t.Slug).HasColumnType("varchar").HasMaxLength(250).IsRequired().HasComment("Slug");
            builder.Property(t => t.ParentCategoryId).HasComment("ParentCategoryId");
            builder.Property(t => t.TaxCategoryId).HasComment("TaxCategoryId");
            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");

            builder.HasOne(x => x.Parent).WithMany(x => x.Childs).HasForeignKey(x => x.ParentCategoryId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
