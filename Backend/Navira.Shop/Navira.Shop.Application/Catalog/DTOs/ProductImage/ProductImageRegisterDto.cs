namespace Navira.Shop.Application.Catalog
{


    public class ProductImageRegisterDto
    {
        public int ProductId { get; set; }
        public int? ProductVariantId { get; set; }
        public string AltText { get; set; }
        public int SortOrder { get; set; }
        public bool IsPrimary { get; set; }
        public Guid MediaFileId { get; set; }

    }
}
