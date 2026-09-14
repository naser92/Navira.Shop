using Navira.Shop.Application.Catalog.DTOs.ProductAttribute;
using Navira.Shop.Core.Dapper;

namespace Navira.Shop.Application.Catalog
{
    public interface IProductAttributeDapperService : IDapperService
    {
        Task<object> GetList(ProductAttributeGetFilterDto filter);
    }
}
