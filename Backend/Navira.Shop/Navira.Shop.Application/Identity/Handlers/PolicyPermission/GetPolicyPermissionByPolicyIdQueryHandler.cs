using Navira.Shop.Application.Identity.ServicesContract;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Identity
{
    public class GetPolicyPermissionByPolicyIdQueryHandler : CommandHandler, IQueryHandler<GetPolicyPermissionByPolicyIdCommand, IEnumerable<PolicyPermissionGetByPolicyIdDto>>
    {
        private readonly IPolicyPermissionQueryService _policyPermissionQueryService;
        public GetPolicyPermissionByPolicyIdQueryHandler(IUnitOfWork uow, IPolicyPermissionQueryService policyPermissionQueryService) : base(uow)
        {
            _policyPermissionQueryService = policyPermissionQueryService;
        }

        public async Task<IResult<IEnumerable<PolicyPermissionGetByPolicyIdDto>>> Handle(GetPolicyPermissionByPolicyIdCommand query, CancellationToken cancellationToken = default)
        {
            return await _policyPermissionQueryService.GetByPolicyId(query.PolicyId).ResultAsync();
        }
    }
}
