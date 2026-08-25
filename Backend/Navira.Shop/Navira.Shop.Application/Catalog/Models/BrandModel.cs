using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;

namespace Navira.Shop.Application.Catalog
{
    public class BrandModel : BaseReadModel<int>, IAuditableEntity
    {

        public string Name { get; set; }

        public string Slug { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ProductModel> Product { get; set; }


    }
}

