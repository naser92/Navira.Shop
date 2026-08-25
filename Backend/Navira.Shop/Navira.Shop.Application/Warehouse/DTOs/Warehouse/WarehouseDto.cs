using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Warehouse
{


    public class WarehouseDto : BaseDto<int>
    {

        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

    }
}
