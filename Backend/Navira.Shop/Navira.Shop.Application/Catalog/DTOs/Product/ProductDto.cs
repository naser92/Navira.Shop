using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{


    public class ProductDto : BaseDto<int>
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

    }
}
