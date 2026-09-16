using Navira.Shop.Application.Common;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeOptionGetFilterDto : PaginationModel
    {
        public int ProductAttributeId { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
    }
}
