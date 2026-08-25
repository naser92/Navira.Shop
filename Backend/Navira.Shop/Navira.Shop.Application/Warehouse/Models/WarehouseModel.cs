using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;

namespace Navira.Shop.Application.Warehouse
{
    public class WarehouseModel : BaseReadModel<int>, IAuditableEntity
    {

        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<StockModel> Stock { get; set; }


        public virtual ICollection<StockMovementModel> StockMovement { get; set; }


    }
}

