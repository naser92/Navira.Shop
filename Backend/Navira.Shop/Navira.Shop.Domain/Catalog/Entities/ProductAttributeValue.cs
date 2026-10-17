using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Catalog
{
    public class ProductAttributeValue : FullEntity<long>, IFullAuditableEntity<Guid>
    {
        public int ProductId { get; set; }
        public int ProductAttributeId { get; set; }
        public int? ProductAttributeOptionId { get; set; }
        public string TextValue { get; set; }
        public decimal? NumberValue { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime? DateValue { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = default!;
        [ForeignKey(nameof(ProductAttributeId))]
        public virtual ProductAttribute ProductAttribute { get; set; } = default!;
        public virtual ProductAttributeOption? ProductAttributeOption { get; set; }
        public virtual ICollection<ProductAttributeValueOption> ProductAttributeValueOption { get; set; } = new List<ProductAttributeValueOption>();


        public ProductAttributeValue() { }

        public ProductAttributeValue(int productId, int productAttributeId, int? productAttributeOptionId = null, string textValue = null, decimal? numberValue = null, bool? booleanValue = null, DateTime? dateValue = null)
        {
            ProductId = productId;
            ProductAttributeId = productAttributeId;
            ProductAttributeOptionId = productAttributeOptionId;
            TextValue = textValue;
            NumberValue = numberValue;
            BooleanValue = booleanValue;
            DateValue = dateValue;
        }

        //public ProductAttributeValue(int productId, ProductAttribute productAttribute, string textValue = null, decimal? numberValue = null, bool? booleanValue = null, DateTime? dateValue = null)
        //{
        //    ProductId = productId;
        //    ProductAttribute = productAttribute;
        //    TextValue = textValue;
        //    NumberValue = numberValue;
        //    BooleanValue = booleanValue;
        //    DateValue = dateValue;
        //}


        public static ProductAttributeValue Create(int productId, int productAttributeId, int? productAttributeOptionId = null, string textValue = null, decimal? numberValue = null, bool? booleanValue = null, DateTime? dateValue = null) =>
            new ProductAttributeValue(
                    productId,
                    productAttributeId,
                    productAttributeOptionId,
                    textValue,
                    numberValue,
                    booleanValue,
                    dateValue
                );





    }
}
