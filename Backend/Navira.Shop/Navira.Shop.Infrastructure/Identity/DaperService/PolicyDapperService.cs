using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Dapper;
using NaviraShop.Core.Mq;
using System.Data;

namespace Navira.Shop.Infrastructure.Identity
{
    public class PolicyDapperService : DapperBaseService, IPolicyDapperService
    {
        public PolicyDapperService([FromKeyedServices("DapperConnection")] IDbConnection dbConnection, IPublisher publisher) : base(dbConnection, publisher)
        {
        }

        public async Task<object> GetList(PolicyGetFilterDto filter) =>
           await QueryMultiple(
                  @$"SP_Policy_list", new DynamicParameters(filter)
                  , (data) =>
                  {
                      return new
                      {
                          data = data.Read<PolicyDto>().ToList(),
                          totalCount = data.ReadFirst<int>()
                      };
                  });

    }
}