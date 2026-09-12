using Navira.Shop.Core.Dapper;

namespace Navira.Shop.Application.Identity
{
    public interface IPermissionDapperService : IDapperService
    {
        Task<object> GetList(PermissionGetFilterDto filter);
    }
}
