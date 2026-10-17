namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeSelectedOptionDto
    {
        // شناسه ProductAttributeOption
        public int Id { get; set; }

        public string Value { get; set; } = default!;

        public string? Code { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
        public long ProductAttributeValueId { get; set; }
    }
}
