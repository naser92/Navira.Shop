using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeDeleteCommandHandler : CommandHandler, ICommandHandler<ProductAttributeDeleteCommand>
    {
        private readonly IProductAttributeWriteRepository _repository;

        public ProductAttributeDeleteCommandHandler(IProductAttributeWriteRepository repository, IUnitOfWork uow) : base(uow)
        {

            _repository = repository;

        }

        public async Task<IResult> Handle(ProductAttributeDeleteCommand command, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.Get(command.Id);

            if (entity is null)
                return await Result.FailAsync("اطلاعات با این مشخصات پیدا نشد.");

            await _repository.Delete(entity);
            return await Result.SuccessAsync("اطلاعات با موفقیت حذف شد");
        }
    }
}
