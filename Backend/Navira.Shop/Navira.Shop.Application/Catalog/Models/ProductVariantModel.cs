using Navira.Shop.Application.Common;
using Navira.Shop.Application.Warehouse;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Catalog
{
    public class ProductVariantModel : BaseReadModel<int>, IAuditableEntity
    {

        public int ProductId { get; set; }

        public string Sku { get; set; }

        public decimal Price { get; set; }

        public decimal? CostPrice { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ProductId")]
        public virtual ProductModel Product { get; set; }

        public virtual ICollection<ProductVariantAttributeValueModel> ProductVariantAttributeValue { get; set; }


        public virtual ICollection<StockModel> Stock { get; set; }


        public virtual ICollection<StockMovementModel> StockMovement { get; set; }


        public virtual ICollection<ProductImageModel> ProductImage { get; set; }


    }
}

