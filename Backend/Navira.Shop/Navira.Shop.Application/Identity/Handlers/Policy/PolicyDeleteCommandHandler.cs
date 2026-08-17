using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Application.Identity.Handlers.Policy
{
    public class PolicyDeleteCommandHandler : CommandHandler, ICommandHandler<PolicyDeleteCommand>
    {
        private readonly IPolicyWriteRepository _repository;

        public PolicyDeleteCommandHandler(IPolicyWriteRepository repository, IUnitOfWork uow) : base(uow)
        {

            _repository = repository;

        }

        public async Task<IResult> Handle(PolicyDeleteCommand command, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.Get(command.Id);
            if (entity is null)
                return await Result.FailAsync("اطلاعات با این مشخصات پیدا نشد.");
            await _repository.Delete(entity);
            return await Result.SuccessAsync("اطلاعات با موفقیت حذف شد");
        }
    }
}
