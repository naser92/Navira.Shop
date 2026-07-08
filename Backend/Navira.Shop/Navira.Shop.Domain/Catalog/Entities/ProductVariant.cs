using Navira.Shop.Core.Domain;
using Navira.Shop.Domain.Catalog.ValueObjects;

namespace Navira.Shop.Domain.Catalog.Entities
{
    /// <summary>
    /// A purchasable SKU, owned by a Product. This is a child Entity (has identity,
    /// persisted as its own row) but NOT an aggregate root — no repository of its own,
    /// never loaded/saved independently of its parent Product.
    /// </summary>
    public sealed class ProductVariant : Entity<int>
    {
        private readonly List<ProductVariantAttributeValue> _attributeValues = new();

        public int ProductId { get; private set; }
        public Sku Sku { get; private set; } = default!;
        public string? Barcode { get; private set; }
        public Money Price { get; private set; } = default!;
        public Money? CostPrice { get; private set; }
        public decimal WeightKg { get; private set; }
        public bool IsActive { get; private set; } = true;

        public IReadOnlyCollection<ProductVariantAttributeValue> AttributeValues => _attributeValues.AsReadOnly();

        private ProductVariant() { }

        private ProductVariant(int productId, Sku sku, Money price, decimal weightKg, Money? costPrice)
        {
            ProductId = productId;
            Sku = sku;
            Price = price;
            WeightKg = weightKg < 0 ? throw new DomainException("Weight cannot be negative.") : weightKg;
            CostPrice = costPrice;
        }

        /// <summary>Internal factory — only Product.AddVariant should call this, keeping
        /// creation funneled through the aggregate root's invariant checks.</summary>
        internal static ProductVariant Create(int productId, Sku sku, Money price, decimal weightKg, Money? costPrice) =>
            new(productId, sku, price, weightKg, costPrice);

        internal void ChangePrice(Money newPrice)
        {
            if (newPrice.Amount <= 0)
                throw new DomainException($"Variant '{Sku}' price must be greater than zero.");
            Price = newPrice;
        }

        internal void SetBarcode(string? barcode) => Barcode = barcode;

        internal void Deactivate() => IsActive = false;

        internal void Activate() => IsActive = true;

        //internal void SetAttributeValue(ProductVariantAttributeValue value)
        //{
        //    _attributeValues.RemoveAll(v => v.ProductAttributeId == value.ProductAttributeId);
        //    _attributeValues.Add(value);
        //}
    }

}
