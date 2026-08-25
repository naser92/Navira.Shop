using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Catalog
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class BrandRegisterDto
    {


        /// <summary>
        ///  Name
        /// </summary>
        [Display(Name = "Name")]
        public string Name { get; set; }


        /// <summary>
        ///  Slug
        /// </summary>
        [Display(Name = "Slug")]
        public string Slug { get; set; }


        /// <summary>
        ///  وضعیت اعتبار
        /// </summary>
        [Display(Name = "وضعیت اعتبار")]
        public bool IsActive { get; set; }

    }
}
