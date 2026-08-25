using Microsoft.EntityFrameworkCore;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Results;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{
    public class ProductVariantAttributeValueQueryService : IProductVariantAttributeValueQueryService
    {

        #region variables

        private readonly IProductVariantAttributeValueQueryRepository _repository;

        #endregion

        #region constructors

        public ProductVariantAttributeValueQueryService(IProductVariantAttributeValueQueryRepository productvariantattributevalueRepository)
        {
            _repository = productvariantattributevalueRepository;
        }

        #endregion

        #region base methods

        public async Task<IResult<TResult>> Get<TResult>(long id)
        {

            return await _repository.Get<TResult>(x => x.Id.Equals(id)).ResultAsync();

        }

        public async Task<IResult<object>> Get<T>(GridParameters parameters)//SortedGridParameters parameters
        {

            var query = _repository.Table;
            var result = await query.ProjectTo<T>().GridAsync(parameters)
                .ResultAsync();

            return result;
        }

        public async Task<IResult<IList<T>>> Get<T>()
        {

            var query = _repository.Table;//.OrderBy(x => x.Priority);

            var result = await query.ProjectTo<T>().ToListAsync()
                .ResultAsync();

            return result;
        }


        #endregion

    }

}

