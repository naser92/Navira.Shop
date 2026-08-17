using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Identity;


namespace Navira.Shop.Application.Identity.Handlers.Policy
{
    public class PolicyRegisterCommandHandler : CommandHandler, ICommandHandler<PolicyRegisterCommand>
    {
        private readonly IPolicyWriteRepository _policyWriteRepository;
        public PolicyRegisterCommandHandler(IUnitOfWork uow, IPolicyWriteRepository policyWriteRepository) : base(uow)
        {
            _policyWriteRepository = policyWriteRepository;
        }

        public async Task<IResult> Handle(PolicyRegisterCommand command, CancellationToken cancellationToken = default)
        {
            var policy = Domain.Identity.Policy.Create(
                command.Name,
                command.Title,
                command.Description
                );

            await _policyWriteRepository.Insert(policy);

            return await Result.SuccessAsync("اطلاعات با موفقیت ثبت شد");
        }
    }
}
