using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Ecommerce
{


    public class TaxCategoryDto : BaseDto<int>
    {

        public string Name { get; set; }

        public bool IsActive { get; set; }

    }
}
