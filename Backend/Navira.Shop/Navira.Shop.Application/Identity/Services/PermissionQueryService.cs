using Microsoft.EntityFrameworkCore;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Results;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Identity
{
    public class PermissionQueryService : IPermissionQueryService
    {

        #region variables

        private readonly IPermissionQueryRepository _repository;

        #endregion

        #region constructors

        public PermissionQueryService(IPermissionQueryRepository permissionRepository)
        {
            _repository = permissionRepository;
        }

        #endregion

        #region base methods

        public async Task<IResult<TResult>> Get<TResult>(int id)
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


        public async Task<IEnumerable<T>> GetByCode<T>(List<string> code) =>
            await _repository.GetAll<T>(x => x.Where(i => code.Contains(i.Code)));


        public async Task<T> GetByCode<T>(string code) => await _repository.Get<T>(i => i.Code == code);


        #endregion

    }
}
