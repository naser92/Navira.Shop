using Navira.Shop.Core.Domain;
using Navira.Shop.Domain.Catalog;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Warehouse
{
    public class Stock : FullEntity<long>, IFullAuditableEntity<Guid>
    {

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariant ProductVariant { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        public int ProductVariantId { get; set; }

        public int WarehouseId { get; set; }

        public int QuantityOnHand { get; set; }

        public int QuantityReserved { get; set; }

    }
}
