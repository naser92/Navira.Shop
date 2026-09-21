using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{
    public class CategorySortListDto : BaseDto<int>
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public int? ParentCategoryId { get; set; }

        public int? TaxCategoryId { get; set; }

        public bool IsActive { get; set; }

        public List<CategorySortListDto> Child { get; set; } = new List<CategorySortListDto>();
    }
}
