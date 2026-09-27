using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Warehouses;

namespace Navira.Shop.Application.Warehouses
{
    public class WarehouseRegisterCommandHandler : CommandHandler, ICommandHandler<WarehouseRegisterCommand>
    {
        private readonly IWarehouseWriteRepository _repository;

        public WarehouseRegisterCommandHandler(IWarehouseWriteRepository repository, IUnitOfWork uow) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(WarehouseRegisterCommand command, CancellationToken cancellationToken)
        {
            var entiy = command.Map<Warehouse>();
            await _repository.Insert(entiy);
            return await Result.SuccessAsync("اطلاعات با موفقیت ثبت شد");
        }
    }
}
