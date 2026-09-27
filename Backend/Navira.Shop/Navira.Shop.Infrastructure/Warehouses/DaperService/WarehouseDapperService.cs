using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Application.Warehouses;
using Navira.Shop.Core.Dapper;
using NaviraShop.Core.Mq;
using System.Data;

namespace Navira.Shop.Infrastructure.Warehouses
{
    public class WarehouseDapperService : DapperBaseService, IWarehouseDapperService
    {
        public WarehouseDapperService([FromKeyedServices("DapperConnection")] IDbConnection dbConnection, IPublisher publisher) : base(dbConnection, publisher)
        {
        }

        public async Task<object> GetList(WarehouseListFilterDto filter) =>
                await QueryMultiple(
                  @$"SP_Warehouse_list", new DynamicParameters(filter)
                  , (data) =>
                  {
                      return new
                      {
                          data = data.Read<WarehouseDto>().ToList(),
                          totalCount = data.ReadFirst<int>()
                      };
                  });
    }
}
