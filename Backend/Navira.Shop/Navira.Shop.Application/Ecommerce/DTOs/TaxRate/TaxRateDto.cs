using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Ecommerce
{


    public class TaxRateDto : BaseDto<int>
    {

        public int TaxCategoryId { get; set; }

        public decimal Percentage { get; set; }

        public DateTime EffectiveFromUtc { get; set; }

        public DateTime? EffectiveToUtc { get; set; }

    }
}
