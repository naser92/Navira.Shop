using Navira.Shop.Application.Catalog;
using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Warehouse
{
    public class StockModel : BaseReadModel<long>, IAuditableEntity
    {

        public int ProductVariantId { get; set; }

        public int WarehouseId { get; set; }

        public int QuantityOnHand { get; set; }

        public int QuantityReserved { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariantModel ProductVariant { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual WarehouseModel Warehouse { get; set; }

    }
}

