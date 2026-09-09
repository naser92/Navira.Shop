using Navira.Shop.Application.Common;

namespace Navira.Shop.Application.Catalog
{
    public class BrandGetFilterDto : PaginationModel
    {

        public string Name { get; set; }

        public string Slug { get; set; }

        public bool? IsActive { get; set; }
    }
}
