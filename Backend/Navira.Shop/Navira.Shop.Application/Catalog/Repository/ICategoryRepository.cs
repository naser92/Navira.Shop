using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Catalog.Entities;

namespace Navira.Shop.Application.Catalog.Repository
{
    public interface ICategoryRepository : IWriteRepository<Category,int>
    {
    }
}
