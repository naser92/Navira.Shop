using Navira.Shop.Core.Domain;
using Navira.Shop.Domain.Warehouses;
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

        public virtual ICollection<ProductVariantAttributeValue> ProductVariantAttributeValue { get; set; } = new List<ProductVariantAttributeValue>();

        public virtual ICollection<Stock> Stock { get; set; }

        public virtual ICollection<StockMovement> StockMovement { get; set; }

        public virtual ICollection<ProductImage> ProductImage { get; set; }

        private ProductVariant() { }

        private ProductVariant(int productId, string sku, decimal? price, decimal? costPrice, bool isActive = true)
        {
            ProductId = productId;
            Sku = sku;
            Price = price ?? 0;
            CostPrice = costPrice;
            IsActive = isActive;
        }


        public static ProductVariant Create(int productId, string sku, decimal? price, decimal? costPrice, bool isActive = true) =>
            new(productId, sku, price, costPrice, isActive);

        public void AddOption(int productAttributeId, int productAttributeOptionId)
        {
            if (ProductVariantAttributeValue.Any(x => x.ProductAttributeId == productAttributeId))
                throw new InvalidOperationException("برای هر ویژگی فقط یک گزینه قابل ثبت است.");

            ProductVariantAttributeValue.Add(new ProductVariantAttributeValue(productAttributeId, productAttributeOptionId));
        }

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
