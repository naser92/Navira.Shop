using Navira.Shop.Core.Results;
using Navira.Shop.Core.Service;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{

    public interface IProductImageQueryService : IBaseService
    {
        #region base methods
        Task<IResult<T>> Get<T>(long id);
        Task<IResult<object>> Get<T>(GridParameters parameters);
        Task<IResult<IList<T>>> Get<T>();
        #endregion
    }

}
