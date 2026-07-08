namespace Navira.Shop.Application.Catalog.DTOs
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public int TaxCategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        public int DisplayOrder { get; set; }

    }
}
