using Navira.Shop.Core.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Catalog
{
    public class CategoryListDto : BaseDto<int>
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
        ///  شناسه 
        /// </summary>
        [Display(Name = "شناسه ")]
        public int? ParentCategoryId { get; set; }

        /// <summary>
        ///  عنوان 
        /// </summary>
        [Display(Name = "عنوان ")]
        public string ParentCategoryName { get; set; }

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
