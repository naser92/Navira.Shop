namespace Navira.Shop.Application.Catalog
{
    public class CategoryChangeParentDto
    {
        public int SourceCategoryId { get; set; }
        public int DesCategoryId { get; set; }
        public bool IsChangeChild { get; set; }
    }
}
