using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{


    public class ProductAttributeOptionDto : BaseDto<int>
    {

        public int ProductAttributeId { get; set; }

        public string Value { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

    }
}
