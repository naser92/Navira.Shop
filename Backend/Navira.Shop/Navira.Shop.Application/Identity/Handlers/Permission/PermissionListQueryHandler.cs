using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Identity
{
    public class PermissionListQueryHandler : CommandHandler, IQueryHandler<PermissionListCommand, object>
    {
        private IPermissionDapperService _permissionDapperService;
        public PermissionListQueryHandler(IUnitOfWork uow, IPermissionDapperService permissionDapperService) : base(uow)
        {

            _permissionDapperService = permissionDapperService;
        }

        public async Task<IResult<object>> Handle(PermissionListCommand query, CancellationToken cancellationToken = default)
        {
            return await _permissionDapperService.GetList(query).ResultAsync();
        }
    }
}
