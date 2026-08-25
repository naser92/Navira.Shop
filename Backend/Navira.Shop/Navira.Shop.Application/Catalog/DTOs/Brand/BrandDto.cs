using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{


    public class BrandDto : BaseDto<int>
    {

        public string Name { get; set; }

        public string Slug { get; set; }

        public bool IsActive { get; set; }

    }
}
