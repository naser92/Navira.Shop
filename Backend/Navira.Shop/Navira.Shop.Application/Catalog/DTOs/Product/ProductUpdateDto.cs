using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{

    public class ProductUpdateDto : BaseDto<int>
    {

        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? TaxCategoryId { get; set; }
        public bool IsActive { get; set; }

    }
}
