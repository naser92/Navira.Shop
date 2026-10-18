namespace Navira.Shop.Application.Catalog;

public class ProductAttributesResponseDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = default!;

    public List<ProductAttributeValueResponseDto> Attributes { get; set; } = [];
}
