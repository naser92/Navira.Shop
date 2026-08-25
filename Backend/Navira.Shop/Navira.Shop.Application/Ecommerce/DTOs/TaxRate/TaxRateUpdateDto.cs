using Navira.Shop.Core.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Ecommerce
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class TaxRateUpdateDto : BaseDto<int>
    {

        /// <summary>
        ///  TaxCategoryId
        /// </summary>
        [Display(Name = "TaxCategoryId")]
        public int TaxCategoryId { get; set; }

        /// <summary>
        ///  Percentage
        /// </summary>
        [Display(Name = "Percentage")]
        public decimal Percentage { get; set; }

        /// <summary>
        ///  EffectiveFromUtc
        /// </summary>
        [Display(Name = "EffectiveFromUtc")]
        public DateTime EffectiveFromUtc { get; set; }

        /// <summary>
        ///  EffectiveToUtc
        /// </summary>
        [Display(Name = "EffectiveToUtc")]
        public DateTime? EffectiveToUtc { get; set; }

    }
}
