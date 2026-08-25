using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Catalog
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class ProductVariantAttributeValueRegisterDto
    {


        /// <summary>
        ///  ProductVariantId
        /// </summary>
        [Display(Name = "ProductVariantId")]
        public int ProductVariantId { get; set; }


        /// <summary>
        ///  ProductAttributeId
        /// </summary>
        [Display(Name = "ProductAttributeId")]
        public int ProductAttributeId { get; set; }


        /// <summary>
        ///  ProductAttributeOptionId
        /// </summary>
        [Display(Name = "ProductAttributeOptionId")]
        public int ProductAttributeOptionId { get; set; }

    }
}
