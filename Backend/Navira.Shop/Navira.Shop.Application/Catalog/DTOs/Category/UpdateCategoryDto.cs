namespace Navira.Shop.Application.Catalog.DTOs
{
    public class UpdateCategoryDto
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public int TaxCategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

    }
}
