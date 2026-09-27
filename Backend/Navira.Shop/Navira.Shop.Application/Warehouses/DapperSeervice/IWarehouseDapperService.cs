using Navira.Shop.Core.Dapper;

namespace Navira.Shop.Application.Warehouses
{
    public interface IWarehouseDapperService : IDapperService
    {
        Task<object> GetList(WarehouseListFilterDto filter);
    }
}
