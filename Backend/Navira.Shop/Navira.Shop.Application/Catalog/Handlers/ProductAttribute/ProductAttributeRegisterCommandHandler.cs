using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeRegisterCommandHandler : CommandHandler, ICommandHandler<ProductAttributeRegisterCommand>
    {
        private readonly IProductAttributeWriteRepository _repository;
        public ProductAttributeRegisterCommandHandler(IUnitOfWork uow, IProductAttributeWriteRepository repository) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(ProductAttributeRegisterCommand command, CancellationToken cancellationToken = default)
        {
            var entiy = command.Map<ProductAttribute>();
            await _repository.Insert(entiy);
            return await Result.SuccessAsync("اطلاعات با موفقیت ثبت شد");
        }
    }
}
