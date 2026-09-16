
using Navira.Shop.Core.Dapper;

namespace Navira.Shop.Application.Catalog
{
    public interface IProductAttributeOptionDapperService : IDapperService
    {
        Task<object> GetList(ProductAttributeOptionGetFilterDto filter);
    }
}
