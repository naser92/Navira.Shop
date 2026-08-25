using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Catalog
{
    public class CategoryModel : BaseReadModel<int>, IAuditableEntity
    {

        public string Name { get; set; }

        public string Slug { get; set; }

        public int? ParentCategoryId { get; set; }

        public int? TaxCategoryId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParentCategoryId")]
        public virtual CategoryModel Parent { get; set; }

        public virtual ICollection<CategoryModel> Childs { get; set; }

        public virtual ICollection<ProductModel> Product { get; set; }


    }
}

