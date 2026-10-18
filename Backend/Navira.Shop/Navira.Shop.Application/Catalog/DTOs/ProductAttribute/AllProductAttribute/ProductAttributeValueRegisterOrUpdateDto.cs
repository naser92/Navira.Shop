namespace Navira.Shop.Application.Catalog;

public class ProductAttributeValueRegisterOrUpdateDto
{
    public int ProductAttributeId { get; set; }

    public string? TextValue { get; set; }

    public decimal? NumberValue { get; set; }

    public bool? BooleanValue { get; set; }

    public DateTime? DateValue { get; set; }
    public int? ProductAttributeOptionId { get; set; }

    public List<int> ProductAttributeOptionIds { get; set; } = [];
}
