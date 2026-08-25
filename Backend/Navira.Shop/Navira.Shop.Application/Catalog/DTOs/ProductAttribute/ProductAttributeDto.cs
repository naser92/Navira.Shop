using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{


    public class ProductAttributeDto : BaseDto<int>
    {

        public string Name { get; set; }

        public string ValueType { get; set; }

        public string Usage { get; set; }

        public bool IsActive { get; set; }

    }
}
