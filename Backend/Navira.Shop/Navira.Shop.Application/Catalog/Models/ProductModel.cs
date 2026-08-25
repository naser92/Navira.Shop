using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Catalog
{
    public class ProductModel : BaseReadModel<int>, IAuditableEntity
    {

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

        [ForeignKey("BrandId")]
        public virtual BrandModel Brand { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryModel Category { get; set; }

        public virtual ICollection<ProductVariantModel> ProductVariant { get; set; }


        public virtual ICollection<ProductImageModel> ProductImage { get; set; }


    }
}

