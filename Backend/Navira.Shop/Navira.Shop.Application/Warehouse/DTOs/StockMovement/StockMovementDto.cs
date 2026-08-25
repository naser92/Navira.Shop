using System;
using System.ComponentModel.DataAnnotations;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.ApplicationService.Dto
{

 
    public class StockMovementDto : BaseDto<long>
    {

        public int ProductVariantId { get; set; }

        public int WarehouseId { get; set; }

        public string MovementType { get; set; }

        public int Quantity { get; set; }

        public string ReferenceType { get; set; }

        public long? ReferenceId { get; set; }

        public string Description { get; set; }

    }
}
