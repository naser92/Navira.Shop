using Navira.Shop.Core.ViewModels;
using Navira.Shop.Domain.Catalog;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Catalog
{


    [Display(Name = "", Description = "")]
    public class ProductAttributeUpdateDto : BaseDto<int>
    {

        public string Name { get; set; }

        public AttributeValueType ValueType { get; set; }

        public bool IsVariantAttribute { get; set; }

        public bool IsFilterable { get; set; }

        public bool IsVisible { get; set; }

        public bool IsSearchable { get; set; }

        public bool IsActive { get; set; }

    }
}
