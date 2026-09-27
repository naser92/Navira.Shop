namespace Navira.Shop.Application.Catalog
{


    public class ProductVariantRegisterDto
    {

        public int ProductId { get; set; }

        public string Sku { get; set; }

        public decimal Price { get; set; }

        public decimal? CostPrice { get; set; }

        public bool IsActive { get; set; }
        public List<ProductVariantOptionRegisterDto> Options { get; set; } = [];
    }
}
