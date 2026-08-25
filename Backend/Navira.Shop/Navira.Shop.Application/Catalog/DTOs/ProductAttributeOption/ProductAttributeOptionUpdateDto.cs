using Navira.Shop.Core.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Catalog
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class ProductAttributeOptionUpdateDto : BaseDto<int>
    {

        /// <summary>
        ///  ProductAttributeId
        /// </summary>
        [Display(Name = "ProductAttributeId")]
        public int ProductAttributeId { get; set; }

        /// <summary>
        ///  Value
        /// </summary>
        [Display(Name = "Value")]
        public string Value { get; set; }

        /// <summary>
        ///  SortOrder
        /// </summary>
        [Display(Name = "SortOrder")]
        public int SortOrder { get; set; }

        /// <summary>
        ///  وضعیت اعتبار
        /// </summary>
        [Display(Name = "وضعیت اعتبار")]
        public bool IsActive { get; set; }

    }
}
