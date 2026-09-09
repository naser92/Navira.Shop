using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Dapper;
using NaviraShop.Core.Mq;
using System.Data;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class BrandDapperService : DapperBaseService, IBrandDapperService
    {
        public BrandDapperService([FromKeyedServices("DapperConnection")] IDbConnection dbConnection, IPublisher publisher) : base(dbConnection, publisher)
        {
        }

        public async Task<object> GetList(BrandGetFilterDto filter) =>
             await QueryMultiple(
                  @$"SP_Brand_list", new DynamicParameters(filter)
                  , (data) =>
                  {
                      return new
                      {
                          data = data.Read<BrandDto>().ToList(),
                          totalCount = data.ReadFirst<int>()
                      };
                  });

    }
}
