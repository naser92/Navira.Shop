using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Catalog.Entities;

namespace Navira.Shop.Infrastructure.Catalog.Mappers
{
    public class CategoryMapper : EntityMapperBase<Category, int>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            // Always call base first — sets up Id PK, soft-delete default, etc.
            base.Configure(builder);

            builder.ToTable("Categories");

            // Private-setter properties need field access mode so EF can materialize them.
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

            // Self-referencing parent relationship.
            // DeleteBehavior.Restrict prevents cascading deletes removing all children
            // when a parent is soft-deleted — children need their own Delete() calls.
            builder.HasOne<Category>()
                   .WithMany()
                   .HasForeignKey(c => c.ParentCategoryId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
