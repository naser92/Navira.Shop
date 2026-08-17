using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Application.Identity
{
    public class PolicyUpdateCommandHandler : CommandHandler, ICommandHandler<PolicyUpdateCommand>
    {
        private readonly IPolicyWriteRepository _policyWriteRepository;
        public PolicyUpdateCommandHandler(IUnitOfWork uow, IPolicyWriteRepository policyWriteRepository) : base(uow)
        {
            _policyWriteRepository = policyWriteRepository;
        }

        public async Task<IResult> Handle(PolicyUpdateCommand command, CancellationToken cancellationToken = default)
        {
            var policy = await _policyWriteRepository.Get(command.Id);

            if (policy == null)
                return await Result.FailAsync("اطلاعات با این مشخصات پیدا نشد.");

            try
            {
                command.Map(policy);
                return await Result.SuccessAsync("اطلاعات با موفقیت ویرایش شد");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
