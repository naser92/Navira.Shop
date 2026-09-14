using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{

    public class ProductAttributeRegisterDto
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
