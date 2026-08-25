using Navira.Shop.Application.Catalog;
using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Warehouse
{
    public class StockMovementModel : BaseReadModel<long>, IAuditableEntity
    {

        public int ProductVariantId { get; set; }

        public int WarehouseId { get; set; }

        public string MovementType { get; set; }

        public int Quantity { get; set; }

        public string ReferenceType { get; set; }

        public long? ReferenceId { get; set; }

        public string Description { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariantModel ProductVariant { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual WarehouseModel Warehouse { get; set; }

    }
}

