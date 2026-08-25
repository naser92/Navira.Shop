using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Warehouse
{


    public class StockDto : BaseDto<long>
    {

        public int ProductVariantId { get; set; }

        public int WarehouseId { get; set; }

        public int QuantityOnHand { get; set; }

        public int QuantityReserved { get; set; }

    }
}
