using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeValueModel : BaseReadModel<long>, IAuditableEntity
    {
        public int ProductId { get; set; }
        public int ProductAttributeId { get; set; }
        public int? ProductAttributeOptionId { get; set; }
        public string? TextValue { get; set; }
        public decimal? NumberValue { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime? DateValue { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual ProductModel Product { get; set; } = default!;
        [ForeignKey(nameof(ProductAttributeId))]
        public virtual ProductAttributeModel ProductAttribute { get; set; } = default!;
        public virtual ProductAttributeOptionModel? ProductAttributeOption { get; set; }
        public virtual ICollection<ProductAttributeValueOptionModel> ProductAttributeValueOption { get; set; } = new List<ProductAttributeValueOptionModel>();
    }
}
