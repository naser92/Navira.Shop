using Navira.Shop.Core.Domain;

namespace Navira.Shop.Domain.Ecommerce
{
    public class TaxCategory : FullEntity<int>, IFullAuditableEntity<Guid>
    {

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<TaxRate> TaxRate { get; set; }

    }
}
