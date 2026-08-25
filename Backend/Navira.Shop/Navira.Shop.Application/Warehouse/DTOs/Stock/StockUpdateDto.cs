using Navira.Shop.Core.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Warehouse
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class StockUpdateDto : BaseDto<long>
    {

        /// <summary>
        ///  ProductVariantId
        /// </summary>
        [Display(Name = "ProductVariantId")]
        public int ProductVariantId { get; set; }

        /// <summary>
        ///  WarehouseId
        /// </summary>
        [Display(Name = "WarehouseId")]
        public int WarehouseId { get; set; }

        /// <summary>
        ///  QuantityOnHand
        /// </summary>
        [Display(Name = "QuantityOnHand")]
        public int QuantityOnHand { get; set; }

        /// <summary>
        ///  QuantityReserved
        /// </summary>
        [Display(Name = "QuantityReserved")]
        public int QuantityReserved { get; set; }

    }
}
