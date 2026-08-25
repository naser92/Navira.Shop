using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Catalog
{
    public class ProductImageModel : BaseReadModel<long>, IAuditableEntity
    {

        public int ProductId { get; set; }

        public int? ProductVariantId { get; set; }

        public string Url { get; set; }

        public string AltText { get; set; }

        public int SortOrder { get; set; }

        public bool IsPrimary { get; set; }

        [ForeignKey("ProductId")]
        public virtual ProductModel Product { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariantModel ProductVariant { get; set; }

    }
}

