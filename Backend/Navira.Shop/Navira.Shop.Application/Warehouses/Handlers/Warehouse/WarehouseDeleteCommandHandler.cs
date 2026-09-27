using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Warehouses;

namespace Navira.Shop.Application.Warehouses
{
    public class WarehouseDeleteCommandHandler : CommandHandler, ICommandHandler<WarehouseDeleteCommand>
    {
        private readonly IWarehouseWriteRepository _repository;

        public WarehouseDeleteCommandHandler(IWarehouseWriteRepository repository, IUnitOfWork uow) : base(uow)
        {

            _repository = repository;

        }

        public async Task<IResult> Handle(WarehouseDeleteCommand command, CancellationToken cancellationToken)
        {
            var entity = await _repository.Get(command.Id);
            if (entity is null)
                return await Result.FailAsync("اطلاعات با این مشخصات پیدا نشد.");
            await _repository.Delete(entity);
            return await Result.SuccessAsync("اطلاعات با موفقیت حذف شد");
        }
    }
}
