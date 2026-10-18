namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributesRegisterOrUpdateDto
    {
        public int ProductId { get; set; }
        public List<ProductAttributeValueRegisterOrUpdateDto> Attributes { get; set; } = [];
    }
}
