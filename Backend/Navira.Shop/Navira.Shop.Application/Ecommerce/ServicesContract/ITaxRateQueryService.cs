using Navira.Shop.Core.Results;
using Navira.Shop.Core.Service;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Ecommerce
{

    public interface ITaxRateQueryService : IBaseService
    {
        #region base methods
        Task<IResult<T>> Get<T>(int id);
        Task<IResult<object>> Get<T>(GridParameters parameters);
        Task<IResult<IList<T>>> Get<T>();
        #endregion
    }

}
