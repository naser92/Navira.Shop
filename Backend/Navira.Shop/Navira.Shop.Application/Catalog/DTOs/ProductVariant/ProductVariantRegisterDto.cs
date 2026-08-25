using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Catalog
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class ProductVariantRegisterDto
    {


        /// <summary>
        ///  ProductId
        /// </summary>
        [Display(Name = "ProductId")]
        public int ProductId { get; set; }


        /// <summary>
        ///  Sku
        /// </summary>
        [Display(Name = "Sku")]
        public string Sku { get; set; }


        /// <summary>
        ///  Price
        /// </summary>
        [Display(Name = "Price")]
        public decimal Price { get; set; }


        /// <summary>
        ///  CostPrice
        /// </summary>
        [Display(Name = "CostPrice")]
        public decimal? CostPrice { get; set; }


        /// <summary>
        ///  وضعیت اعتبار
        /// </summary>
        [Display(Name = "وضعیت اعتبار")]
        public bool IsActive { get; set; }

    }
}
