using Navira.Shop.Core.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Ecommerce
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class TaxCategoryUpdateDto : BaseDto<int>
    {

        /// <summary>
        ///  Name
        /// </summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        ///  وضعیت اعتبار
        /// </summary>
        [Display(Name = "وضعیت اعتبار")]
        public bool IsActive { get; set; }

    }
}
