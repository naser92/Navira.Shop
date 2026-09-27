namespace Navira.Shop.Application.Catalog
{


    public class ProductRegisterDto
    {


        public string Name { get; set; }

        public string Slug { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public int? BrandId { get; set; }

        public int? TaxCategoryId { get; set; }

        public bool IsActive { get; set; }


    }
}
