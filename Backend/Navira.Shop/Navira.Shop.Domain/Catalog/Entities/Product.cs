using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Catalog
{

    public class Product : FullEntity<int>, IFullAuditableEntity<Guid>
    {
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Sku { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public int? BrandId { get; set; }

        public int? TaxCategoryId { get; set; }

        public bool IsPublished { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ProductVariant> ProductVariant { get; set; }

        public virtual ICollection<ProductImage> ProductImage { get; set; }

        private Product() { }

        //private Product(string name, string slug, int categoryId, string brand)
        //{
        //    Name = name;
        //    Slug = slug;
        //    CategoryId = categoryId;
        //    Brand = brand;
        //    CreatedOnUtc = DateTime.UtcNow;
        //}

        /// <summary>
        /// The only way to create a Product. Id is left at its default(int) (0) here;
        /// EF assigns the real value on insert via the database identity/sequence and
        /// updates this instance's Id (public setter — see Entity&lt;TId&gt;) after SaveChanges.
        /// </summary>
        //public static Product Create(string name, string slug, int categoryId, string brand = "Navira")
        //{
        //    if (string.IsNullOrWhiteSpace(name))
        //        throw new DomainException("Product name cannot be empty.");
        //    if (string.IsNullOrWhiteSpace(slug))
        //        throw new DomainException("Product slug cannot be empty.");
        //    if (categoryId <= 0)
        //        throw new DomainException("Product must belong to a valid category.");

        //    var product = new Product(name.Trim(), slug.Trim().ToLowerInvariant(), categoryId, brand);
        //    //product.Raise(new ProductCreatedEvent(product.Id, product.Name));
        //    return product;
        //}

        //public void UpdateDescription(string? shortDescription, string? description)
        //{
        //    ShortDescription = shortDescription;
        //    Description = description;
        //    Touch();
        //}

        //public void ChangeCategory(int categoryId)
        //{
        //    if (categoryId <= 0)
        //        throw new DomainException("Product must belong to a valid category.");
        //    CategoryId = categoryId;
        //    Touch();
        //}

        //public void OverrideTaxCategory(int? taxCategoryId)
        //{
        //    TaxCategoryId = taxCategoryId;
        //    Touch();
        //}

        /// <summary>
        /// Adds a purchasable SKU to this product. Enforces SKU uniqueness within the
        /// product; global uniqueness across the catalog is a database constraint, not
        /// something this aggregate can verify in isolation.
        /// </summary>
        //public ProductVariant AddVariant(Sku sku, Money price, decimal weightKg, Money? costPrice = null)
        //{
        //    if (!IsActive)
        //        throw new DomainException($"Cannot add a variant to discontinued product '{Name}'.");

        //    if (_variants.Any(v => v.Sku.Equals(sku)))
        //        throw new DomainException($"Product '{Name}' already has a variant with SKU '{sku}'.");

        //    var variant = ProductVariant.Create(Id, sku, price, weightKg, costPrice);
        //    _variants.Add(variant);
        //    //Raise(new VariantAddedToProductEvent(Id, variant.Id, sku.ToString()));
        //    Touch();
        //    return variant;
        //}

        //public void ChangeVariantPrice(int variantId, Money newPrice)
        //{
        //    var variant = GetVariantOrThrow(variantId);
        //    var oldPrice = variant.Price;
        //    variant.ChangePrice(newPrice);
        //    //Raise(new ProductPriceChangedEvent(Id, variantId, oldPrice.Amount, newPrice.Amount));
        //    Touch();
        //}

        //public void DeactivateVariant(int variantId)
        //{
        //    var variant = GetVariantOrThrow(variantId);
        //    variant.Deactivate();
        //    Touch();
        //}

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

        //public void Discontinue()
        //{
        //    if (!IsActive) return;

        //    IsActive = false;
        //    foreach (var variant in _variants)
        //        variant.Deactivate();

        //    //Raise(new ProductDiscontinuedEvent(Id));
        //    Touch();
        //}

        //public void Reactivate()
        //{
        //    IsActive = true;
        //    Touch();
        //}

        //private ProductVariant GetVariantOrThrow(int variantId) =>
        //    _variants.FirstOrDefault(v => v.Id == variantId)
        //        ?? throw new DomainException($"Product '{Name}' has no variant with id {variantId}.");

        //private void Touch() => UpdatedOnUtc = DateTime.UtcNow;
    }

}
