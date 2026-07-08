using Navira.Shop.Core.Domain;
using Navira.Shop.Core.Entity;

namespace Navira.Shop.Domain.Catalog.Entities
{
    public sealed class Category : AggregateRoot<int>, IFullAuditableEntity<Guid>, ISoftDeletableEntity
    {
        public string Name { get; private set; } = default!;
        public string Slug { get; private set; } = default!;
        public string? Description { get; private set; }
        public int? ParentCategoryId { get; private set; }
        public int TaxCategoryId { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsActive { get; private set; } = true;

        public bool IsDeleted { get; set; }

        private Category() { }

        private Category(string name, string slug, int taxCategoryId, int? parentCategoryId)
        {
            Name = name;
            Slug = slug;
            TaxCategoryId = taxCategoryId;
            ParentCategoryId = parentCategoryId;
        }

        public static Category Create(string name, string slug, int taxCategoryId, int? parentCategoryId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Category name cannot be empty.");
            if (taxCategoryId <= 0)
                throw new DomainException("Category must have a valid tax category.");

            return new Category(name.Trim(), slug.Trim().ToLowerInvariant(), taxCategoryId, parentCategoryId);
        }

        public void Rename(string name, string slug)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Category name cannot be empty.");
            Name = name.Trim();
            Slug = slug.Trim().ToLowerInvariant();
        }

        public void UpdateDescription(string? description) => Description = description;

        public void ChangeTaxCategory(int taxCategoryId)
        {
            if (taxCategoryId <= 0)
                throw new DomainException("Category must have a valid tax category.");
            TaxCategoryId = taxCategoryId;
        }

        public void MoveUnder(int? parentCategoryId)
        {
            if (parentCategoryId == Id)
                throw new DomainException("A category cannot be its own parent.");
            ParentCategoryId = parentCategoryId;
        }

        public void SetDisplayOrder(int displayOrder) => DisplayOrder = displayOrder;

        public void Activate()
        {
            if (IsDeleted)
                throw new DomainException("Cannot activate a deleted category.");
            IsActive = true;
        }

        public void Deactivate() => IsActive = false;

    }
}
