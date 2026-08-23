using Navira.Shop.Application.Identity.Commands.RolePolicy;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Application.Identity
{
    public class AsingeAndUnAsingRolePolicyCommandHandler : CommandHandler, ICommandHandler<AsingeAndUnAsingRolePolicyCommand>
    {
        private readonly IRolePolicyWriteRepository _rolePolicyWriteRepository;
        public AsingeAndUnAsingRolePolicyCommandHandler(IUnitOfWork uow, IRolePolicyWriteRepository rolePolicyWriteRepository) : base(uow)
        {
            _rolePolicyWriteRepository = rolePolicyWriteRepository;
        }

        public async Task<IResult> Handle(AsingeAndUnAsingRolePolicyCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var rolePolicyList = RolePolicy.AssingePolicy(command.RoleId, command.PolicyAsinge);

                await _rolePolicyWriteRepository.Insert(rolePolicyList);

                foreach (var policyId in command.PolicyUnAsinge)
                {
                    var policy = await _rolePolicyWriteRepository.Get(x => x.PolicyId == policyId && x.RoleId == command.RoleId);

                    if (policy is not null)
                        await _rolePolicyWriteRepository.Delete(policy);
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
