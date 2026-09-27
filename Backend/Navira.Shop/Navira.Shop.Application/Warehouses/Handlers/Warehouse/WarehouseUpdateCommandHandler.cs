using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Warehouses;

namespace Navira.Shop.Application.Warehouses.Handlers.Warehouse
{
    public class WarehouseUpdateCommandHandler : CommandHandler, ICommandHandler<WarehouseUpdateCommand>
    {
        private readonly IWarehouseWriteRepository _repository;

        public WarehouseUpdateCommandHandler(
            IWarehouseWriteRepository repository, IUnitOfWork uow) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(WarehouseUpdateCommand command, CancellationToken cancellationToken)
        {
            var entity = await _repository.Get(command.Id);
            if (entity is null)
                return await Result.FailAsync("اطلاعات با این مشخصات پیدا نشد.");
            command.Map(entity);
            await _repository.Update(entity);
            return await Result.SuccessAsync("اطلاعات با موفقیت ویرایش شد");
        }
    }
}
