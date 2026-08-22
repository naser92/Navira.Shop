using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Identity
{
    public class PermissionListQueryHandler : CommandHandler, IQueryHandler<PermissionListCommand, IList<PermissionModelDto>>
    {
        private IPermissionQueryService _permissionQueryService;
        public PermissionListQueryHandler(IUnitOfWork uow, IPermissionQueryService permissionQueryService) : base(uow)
        {
            _permissionQueryService = permissionQueryService;
        }

        public async Task<IResult<IList<PermissionModelDto>>> Handle(PermissionListCommand query, CancellationToken cancellationToken = default)
        {
            return await _permissionQueryService.Get<PermissionModelDto>();
        }
    }
}
