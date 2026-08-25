using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{


    public class ProductVariantAttributeValueDto : BaseDto<long>
    {

        public int ProductVariantId { get; set; }

        public int ProductAttributeId { get; set; }

        public int ProductAttributeOptionId { get; set; }

    }
}
