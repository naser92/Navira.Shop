using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{


    public class ProductVariantDto : BaseDto<int>
    {

        public int ProductId { get; set; }

        public string Sku { get; set; }

        public decimal Price { get; set; }

        public decimal? CostPrice { get; set; }

        public bool IsActive { get; set; }

    }
}
