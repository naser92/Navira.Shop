using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Dapper;
using NaviraShop.Core.Mq;
using System.Data;

namespace Navira.Shop.Infrastructure.Identity
{
    public class PermissionDapperService : DapperBaseService, IPermissionDapperService
    {
        public PermissionDapperService([FromKeyedServices("DapperConnection")] IDbConnection dbConnection, IPublisher publisher) : base(dbConnection, publisher)
        {
        }

        public async Task<object> GetList(PermissionGetFilterDto filter) =>

            await QueryMultiple(
                  @$"SP_Permission_list", new DynamicParameters(filter)
                  , (data) =>
                  {
                      return new
                      {
                          data = data.Read<PermissionModelDto>().ToList(),
                          totalCount = data.ReadFirst<int>()
                      };
                  });
    }
}
