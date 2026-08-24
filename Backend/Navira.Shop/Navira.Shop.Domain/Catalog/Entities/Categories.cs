using Navira.Shop.Core.Domain;
using Navira.Shop.Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Catalog
{
    public  class Categories : FullEntity<int>, IFullAuditableEntity<Guid>
    {
        [ForeignKey("ParentCategoryId")]
        public virtual Categories Parent { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }

        public string Slug { get; set; }

        public int? TaxCategoryId { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        public virtual ICollection<Categories> Childs { get; set; }
        private Categories() { }

        private Categories(string name, string slug, int taxCategoryId, int? parentCategoryId)
        {
            Name = name;
            Slug = slug;
            TaxCategoryId = taxCategoryId;
            ParentCategoryId = parentCategoryId;
        }

        public static Categories Create(string name, string slug, int taxCategoryId, int? parentCategoryId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Category name cannot be empty.");
            if (taxCategoryId <= 0)
                throw new DomainException("Category must have a valid tax category.");

            return new Categories(name.Trim(), slug.Trim().ToLowerInvariant(), taxCategoryId, parentCategoryId);
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
