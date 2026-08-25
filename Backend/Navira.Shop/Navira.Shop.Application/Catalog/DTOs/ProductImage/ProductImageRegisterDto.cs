using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Catalog
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class ProductImageRegisterDto
    {


        /// <summary>
        ///  ProductId
        /// </summary>
        [Display(Name = "ProductId")]
        public int ProductId { get; set; }


        /// <summary>
        ///  ProductVariantId
        /// </summary>
        [Display(Name = "ProductVariantId")]
        public int? ProductVariantId { get; set; }


        /// <summary>
        ///  Url
        /// </summary>
        [Display(Name = "Url")]
        public string Url { get; set; }


        /// <summary>
        ///  AltText
        /// </summary>
        [Display(Name = "AltText")]
        public string AltText { get; set; }


        /// <summary>
        ///  SortOrder
        /// </summary>
        [Display(Name = "SortOrder")]
        public int SortOrder { get; set; }


        /// <summary>
        ///  IsPrimary
        /// </summary>
        [Display(Name = "IsPrimary")]
        public bool IsPrimary { get; set; }

    }
}
