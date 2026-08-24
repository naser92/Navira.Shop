using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Catalog
{
    public class Category : FullEntity<int>, IFullAuditableEntity<Guid>
    {

        [ForeignKey("ParentCategoryId")]
        public virtual Category Parent { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public int? ParentCategoryId { get; set; }

        public int? TaxCategoryId { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Category> Childs { get; set; }

        public virtual ICollection<Product> Product { get; set; }

    }
}
