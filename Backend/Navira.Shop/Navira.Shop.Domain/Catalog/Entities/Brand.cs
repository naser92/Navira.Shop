using Navira.Shop.Core.Domain;

namespace Navira.Shop.Domain.Catalog
{
    public class Brand : FullEntity<int>, IFullAuditableEntity<Guid>
    {

        public string Name { get; set; }

        public string Slug { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Product> Product { get; set; }

    }
}
