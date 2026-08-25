using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Catalog
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class CategoryRegisterDto
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
        ///  ParentCategoryId
        /// </summary>
        [Display(Name = "ParentCategoryId")]
        public int? ParentCategoryId { get; set; }


        /// <summary>
        ///  TaxCategoryId
        /// </summary>
        [Display(Name = "TaxCategoryId")]
        public int? TaxCategoryId { get; set; }


        /// <summary>
        ///  وضعیت اعتبار
        /// </summary>
        [Display(Name = "وضعیت اعتبار")]
        public bool IsActive { get; set; }

    }
}
