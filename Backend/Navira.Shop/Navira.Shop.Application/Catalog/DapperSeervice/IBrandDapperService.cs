using Navira.Shop.Core.Dapper;

namespace Navira.Shop.Application.Catalog
{
    public interface IBrandDapperService : IDapperService
    {
        Task<object> GetList(BrandGetFilterDto filter);
    }
}
