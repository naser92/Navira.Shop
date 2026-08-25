using System;
using System.ComponentModel.DataAnnotations;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.ApplicationService.Dto
{
    
    /// <summary>
    ///  
    /// </summary>
    [Display(Name="",Description="")]
    public class StockMovementRegisterDto
    {


        /// <summary>
        ///  ProductVariantId
        /// </summary>
        [Display(Name="ProductVariantId")]
        public int ProductVariantId { get; set; }


        /// <summary>
        ///  WarehouseId
        /// </summary>
        [Display(Name="WarehouseId")]
        public int WarehouseId { get; set; }


        /// <summary>
        ///  MovementType
        /// </summary>
        [Display(Name="MovementType")]
        public string MovementType { get; set; }


        /// <summary>
        ///  Quantity
        /// </summary>
        [Display(Name="Quantity")]
        public int Quantity { get; set; }


        /// <summary>
        ///  ReferenceType
        /// </summary>
        [Display(Name="ReferenceType")]
        public string ReferenceType { get; set; }


        /// <summary>
        ///  ReferenceId
        /// </summary>
        [Display(Name="ReferenceId")]
        public long? ReferenceId { get; set; }


        /// <summary>
        ///  Description
        /// </summary>
        [Display(Name="Description")]
        public string Description { get; set; }

    }
}
