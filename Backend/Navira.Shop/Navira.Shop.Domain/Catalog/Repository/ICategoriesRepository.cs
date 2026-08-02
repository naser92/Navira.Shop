using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Domain.Catalog
{
    public interface ICategoriesRepository : IWriteRepository<Categories, int>
    {
    }
}
