using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Dapper;
using NaviraShop.Core.Mq;
using System.Data;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductAttributeDapperService : DapperBaseService, IProductAttributeDapperService
    {
        public ProductAttributeDapperService([FromKeyedServices("DapperConnection")] IDbConnection dbConnection, IPublisher publisher) : base(dbConnection, publisher)
        {
        }

        public async Task<object> GetList(ProductAttributeGetFilterDto filter) =>
            await QueryMultiple(
                  @$"SP_ProductAttribute_list", new DynamicParameters(filter)
                  , (data) =>
                  {
                      return new
                      {
                          data = data.Read<ProductAttributeDto>().ToList(),
                          totalCount = data.ReadFirst<int>()
                      };
                  });
    }
}
