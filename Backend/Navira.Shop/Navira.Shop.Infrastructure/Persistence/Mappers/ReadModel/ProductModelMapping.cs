using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Infrastructure.Mappers.ReadModel
{
    public class ProductModelMapping : EntityReadMapperBase<ProductModel, int>, IReadEntityConfiguration
    {

        public override void Configure(EntityTypeBuilder<ProductModel> builder)
        {
            base.Configure(builder);

            builder.HasComment(";");

            builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(300).IsRequired().HasComment("Name");

            builder.Property(t => t.Slug).HasColumnType("varchar").HasMaxLength(350).IsRequired().HasComment("Slug");

            builder.Property(t => t.Sku).HasColumnType("varchar").HasMaxLength(100).IsRequired().HasComment("Sku");

            builder.Property(t => t.ShortDescription).HasColumnType("nvarchar").HasMaxLength(1000).HasComment("ShortDescription");

            builder.Property(t => t.Description).HasColumnType("nvarcharmax").HasMaxLength(-1).HasComment("Description");

            builder.Property(t => t.CategoryId).IsRequired().HasComment("CategoryId");

            builder.Property(t => t.BrandId).HasComment("BrandId");

            builder.Property(t => t.TaxCategoryId).HasComment("TaxCategoryId");

            builder.Property(t => t.IsPublished).IsRequired().HasDefaultValue(true).HasComment("IsPublished");

            builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true).HasComment("وضعیت اعتبار");

            builder.HasOne(x => x.Brand).WithMany(x => x.Product).HasForeignKey(x => x.BrandId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Category).WithMany(x => x.Product).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
