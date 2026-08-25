using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Catalog
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class ProductRegisterDto
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
        ///  Sku
        /// </summary>
        [Display(Name = "Sku")]
        public string Sku { get; set; }


        /// <summary>
        ///  ShortDescription
        /// </summary>
        [Display(Name = "ShortDescription")]
        public string ShortDescription { get; set; }


        /// <summary>
        ///  Description
        /// </summary>
        [Display(Name = "Description")]
        public string Description { get; set; }


        /// <summary>
        ///  CategoryId
        /// </summary>
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }


        /// <summary>
        ///  BrandId
        /// </summary>
        [Display(Name = "BrandId")]
        public int? BrandId { get; set; }


        /// <summary>
        ///  TaxCategoryId
        /// </summary>
        [Display(Name = "TaxCategoryId")]
        public int? TaxCategoryId { get; set; }


        /// <summary>
        ///  IsPublished
        /// </summary>
        [Display(Name = "IsPublished")]
        public bool IsPublished { get; set; }


        /// <summary>
        ///  وضعیت اعتبار
        /// </summary>
        [Display(Name = "وضعیت اعتبار")]
        public bool IsActive { get; set; }

    }
}
