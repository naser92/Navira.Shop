using Microsoft.EntityFrameworkCore;
using Navira.Shop.Application.Identity.ServicesContract;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Results;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Identity
{
    public class PolicyPermissionQueryService : IPolicyPermissionQueryService
    {

        #region variables

        private readonly IPolicyPermissionQueryRepository _repository;

        #endregion

        #region constructors

        public PolicyPermissionQueryService(IPolicyPermissionQueryRepository policypermissionRepository)
        {
            _repository = policypermissionRepository;
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


        #endregion

        public async Task<IEnumerable<PolicyPermissionGetByPolicyIdDto>> GetByPolicyId(int policyId) =>
           await _repository.Table.Where(x => x.PolicyId == policyId).ProjectTo<PolicyPermissionGetByPolicyIdDto>().ToListAsync();


    }
}
