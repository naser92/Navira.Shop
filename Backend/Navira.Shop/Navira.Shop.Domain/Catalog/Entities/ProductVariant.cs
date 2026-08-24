using Navira.Shop.Core.Domain;
using Navira.Shop.Domain.Warehouse;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Catalog
{

    public class ProductVariant : FullEntity<int>, IFullAuditableEntity<Guid>
    {
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int ProductId { get; set; }

        public string Sku { get; set; }

        public decimal Price { get; set; }

        public decimal? CostPrice { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ProductVariantAttributeValue> ProductVariantAttributeValue { get; set; }

        public virtual ICollection<Stock> Stock { get; set; }

        public virtual ICollection<StockMovement> StockMovement { get; set; }

        public virtual ICollection<ProductImage> ProductImage { get; set; }

        private ProductVariant() { }

        //private ProductVariant(int productId, Sku sku, Money price, decimal weightKg, Money? costPrice)
        //{
        //    ProductId = productId;
        //    Sku = sku;
        //    Price = price;
        //    WeightKg = weightKg < 0 ? throw new DomainException("Weight cannot be negative.") : weightKg;
        //    CostPrice = costPrice;
        //}

        /// <summary>Internal factory — only Product.AddVariant should call this, keeping
        /// creation funneled through the aggregate root's invariant checks.</summary>
        //internal static ProductVariant Create(int productId, Sku sku, Money price, decimal weightKg, Money? costPrice) =>
        //    new(productId, sku, price, weightKg, costPrice);

        //internal void ChangePrice(Money newPrice)
        //{
        //    if (newPrice.Amount <= 0)
        //        throw new DomainException($"Variant '{Sku}' price must be greater than zero.");
        //    Price = newPrice;
        //}

        //internal void SetBarcode(string? barcode) => Barcode = barcode;

        //internal void Deactivate() => IsActive = false;

        //internal void Activate() => IsActive = true;

        //internal void SetAttributeValue(ProductVariantAttributeValue value)
        //{
        //    _attributeValues.RemoveAll(v => v.ProductAttributeId == value.ProductAttributeId);
        //    _attributeValues.Add(value);
        //}
    }

}
