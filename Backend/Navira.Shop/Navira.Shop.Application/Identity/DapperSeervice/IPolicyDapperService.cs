using Navira.Shop.Core.Dapper;

namespace Navira.Shop.Application.Identity
{
    public interface IPolicyDapperService : IDapperService
    {
        Task<object> GetList(PolicyGetFilterDto filter);
    }
}
