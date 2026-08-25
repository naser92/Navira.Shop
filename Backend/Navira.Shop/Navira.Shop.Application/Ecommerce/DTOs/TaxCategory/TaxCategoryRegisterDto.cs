using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Ecommerce
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class TaxCategoryRegisterDto
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
