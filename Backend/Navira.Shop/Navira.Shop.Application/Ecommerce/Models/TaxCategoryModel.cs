using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;

namespace Navira.Shop.Application.Ecommerce
{
    public class TaxCategoryModel : BaseReadModel<int>, IAuditableEntity
    {

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<TaxRateModel> TaxRate { get; set; }


    }
}

