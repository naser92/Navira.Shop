using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Application.Identity
{
    public class AsingeAndUnAsingPolicyPermissionCommandHandler : CommandHandler, ICommandHandler<AsingeAndUnAsingPolicyPermissionCommand>
    {
        private readonly IPolicyPermissionWriteRepository _policyPermissionWriteRepository;
        public AsingeAndUnAsingPolicyPermissionCommandHandler(IUnitOfWork uow, IPolicyPermissionWriteRepository policyPermissionWriteRepository) : base(uow)
        {
            _policyPermissionWriteRepository = policyPermissionWriteRepository;
        }

        public async Task<IResult> Handle(AsingeAndUnAsingPolicyPermissionCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = PolicyPermission.AssingePermission(command.PolicyId, command.PermissionAsinge);
                await _policyPermissionWriteRepository.Insert(entity);

                foreach (var item in command.PermissionUnAsinge)
                {
                    var pp = await _policyPermissionWriteRepository.Get(x => x.PolicyId == command.PolicyId && x.PermissionId == item);

                    if (pp != null)
                        await _policyPermissionWriteRepository.Delete(pp);
                }

                return Result.Success("عملیات با موفقیت انجام شد");
            }
            catch
            (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
