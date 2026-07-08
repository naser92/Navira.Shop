using Navira.Shop.Core.Domain;
using Navira.Shop.Domain.Catalog.ValueObjects;

namespace Navira.Shop.Domain.Catalog.Entities
{
    /// <summary>
    /// Aggregate root for the catalog. Owns its ProductVariants and ProductAttributeValues
    /// as part of its consistency boundary: you cannot add a variant, change a price, or
    /// attach an attribute except through methods on this class.
    ///
    /// References Category and TaxCategory by id only (CategoryId / TaxCategoryId) —
    /// those are separate aggregates with their own lifecycle.
    /// </summary>
    public sealed class Product : AggregateRoot<int>
    {
        private readonly List<ProductVariant> _variants = new();
        //private readonly List<ProductAttributeValue> _attributeValues = new();

        public string Name { get; private set; } = default!;
        public string Slug { get; private set; } = default!;
        public string? ShortDescription { get; private set; }
        public string? Description { get; private set; }

        public int CategoryId { get; private set; }
        public int? TaxCategoryId { get; private set; }

        public string Brand { get; private set; } = "Navira";

        public bool IsActive { get; private set; } = true;

        public DateTime CreatedOnUtc { get; private set; }
        public DateTime? UpdatedOnUtc { get; private set; }

        public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();
        //public IReadOnlyCollection<ProductAttributeValue> AttributeValues => _attributeValues.AsReadOnly();

        // EF materialization constructor. Kept private so application code can't bypass
        // invariants by `new Product()`-ing an empty shell; configure EF to use this via
        // backing-field/private-constructor access in OnModelCreating.
        private Product() { }

        private Product(string name, string slug, int categoryId, string brand)
        {
            Name = name;
            Slug = slug;
            CategoryId = categoryId;
            Brand = brand;
            CreatedOnUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// The only way to create a Product. Id is left at its default(int) (0) here;
        /// EF assigns the real value on insert via the database identity/sequence and
        /// updates this instance's Id (public setter — see Entity&lt;TId&gt;) after SaveChanges.
        /// </summary>
        public static Product Create(string name, string slug, int categoryId, string brand = "Navira")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name cannot be empty.");
            if (string.IsNullOrWhiteSpace(slug))
                throw new DomainException("Product slug cannot be empty.");
            if (categoryId <= 0)
                throw new DomainException("Product must belong to a valid category.");

            var product = new Product(name.Trim(), slug.Trim().ToLowerInvariant(), categoryId, brand);
            //product.Raise(new ProductCreatedEvent(product.Id, product.Name));
            return product;
        }

        public void UpdateDescription(string? shortDescription, string? description)
        {
            ShortDescription = shortDescription;
            Description = description;
            Touch();
        }

        public void ChangeCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new DomainException("Product must belong to a valid category.");
            CategoryId = categoryId;
            Touch();
        }

        public void OverrideTaxCategory(int? taxCategoryId)
        {
            TaxCategoryId = taxCategoryId;
            Touch();
        }

        /// <summary>
        /// Adds a purchasable SKU to this product. Enforces SKU uniqueness within the
        /// product; global uniqueness across the catalog is a database constraint, not
        /// something this aggregate can verify in isolation.
        /// </summary>
        public ProductVariant AddVariant(Sku sku, Money price, decimal weightKg, Money? costPrice = null)
        {
            if (!IsActive)
                throw new DomainException($"Cannot add a variant to discontinued product '{Name}'.");

            if (_variants.Any(v => v.Sku.Equals(sku)))
                throw new DomainException($"Product '{Name}' already has a variant with SKU '{sku}'.");

            var variant = ProductVariant.Create(Id, sku, price, weightKg, costPrice);
            _variants.Add(variant);
            //Raise(new VariantAddedToProductEvent(Id, variant.Id, sku.ToString()));
            Touch();
            return variant;
        }

        public void ChangeVariantPrice(int variantId, Money newPrice)
        {
            var variant = GetVariantOrThrow(variantId);
            var oldPrice = variant.Price;
            variant.ChangePrice(newPrice);
            //Raise(new ProductPriceChangedEvent(Id, variantId, oldPrice.Amount, newPrice.Amount));
            Touch();
        }

        public void DeactivateVariant(int variantId)
        {
            var variant = GetVariantOrThrow(variantId);
            variant.Deactivate();
            Touch();
        }

        /// <summary>
        /// Sets an informational (non-variant-defining) attribute value at the product level,
        /// e.g. Material = Ceramic. Variant-defining attributes (Color, Capacity) belong on
        /// ProductVariant.SetAttributeValue instead.
        /// </summary>
        //public void SetAttributeValue(ProductAttributeValue value)
        //{
        //    _attributeValues.RemoveAll(v => v.ProductAttributeId == value.ProductAttributeId);
        //    _attributeValues.Add(value);
        //    Touch();
        //}

        public void Discontinue()
        {
            if (!IsActive) return;

            IsActive = false;
            foreach (var variant in _variants)
                variant.Deactivate();

            //Raise(new ProductDiscontinuedEvent(Id));
            Touch();
        }

        public void Reactivate()
        {
            IsActive = true;
            Touch();
        }

        private ProductVariant GetVariantOrThrow(int variantId) =>
            _variants.FirstOrDefault(v => v.Id == variantId)
                ?? throw new DomainException($"Product '{Name}' has no variant with id {variantId}.");

        private void Touch() => UpdatedOnUtc = DateTime.UtcNow;
    }

}
