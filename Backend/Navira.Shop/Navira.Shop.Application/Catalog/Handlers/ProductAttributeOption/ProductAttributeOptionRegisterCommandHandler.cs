using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeOptionRegisterCommandHandler : CommandHandler, ICommandHandler<ProductAttributeOptionRegisterCommand>
    {
        private readonly IProductAttributeOptionWriteRepository _repository;

        public ProductAttributeOptionRegisterCommandHandler(
            IProductAttributeOptionWriteRepository repository, IUnitOfWork uow) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(ProductAttributeOptionRegisterCommand command, CancellationToken cancellationToken = default)
        {
            var entiy = command.Map<ProductAttributeOption>();
            await _repository.Insert(entiy);
            return await Result.SuccessAsync("اطلاعات با موفقیت ثبت شد");
        }
    }
}
