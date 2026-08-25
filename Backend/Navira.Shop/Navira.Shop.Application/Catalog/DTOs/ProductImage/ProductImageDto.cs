using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{


    public class ProductImageDto : BaseDto<long>
    {

        public int ProductId { get; set; }

        public int? ProductVariantId { get; set; }

        public string Url { get; set; }

        public string AltText { get; set; }

        public int SortOrder { get; set; }

        public bool IsPrimary { get; set; }

    }
}
