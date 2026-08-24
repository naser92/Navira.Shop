using Navira.Shop.Core.Domain;

namespace Navira.Shop.Domain.Warehouse
{
    public class Warehouse : FullEntity<int>, IFullAuditableEntity<Guid>
    {

        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Stock> Stock { get; set; }

        public virtual ICollection<StockMovement> StockMovement { get; set; }

    }
}
