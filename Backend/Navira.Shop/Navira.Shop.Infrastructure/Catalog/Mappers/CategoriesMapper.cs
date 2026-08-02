using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Infrastructure.Catalog.Mappers.Model
{
    public class CategoriesMapper : EntityMapperBase<Categories, int>
    {
        public override void Configure(EntityTypeBuilder<Categories> builder)
        {
            // Always call base first — sets up Id PK, soft-delete default, etc.
            base.Configure(builder);

            builder.ToTable("Categories");

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(c => c.Slug)
                .IsRequired()
                .HasMaxLength(200)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(c => c.Slug).IsUnique();

            builder.Property(c => c.Description)
                .HasMaxLength(2000)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(c => c.TaxCategoryId)
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(c => c.DisplayOrder)
                .HasDefaultValue(0)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(c => c.ParentCategoryId)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<Categories>()
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
