using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeUpdateCommandHandler : CommandHandler, ICommandHandler<ProductAttributeUpdateCommand>
    {
        private readonly IProductAttributeWriteRepository _repository;

        public ProductAttributeUpdateCommandHandler(
            IProductAttributeWriteRepository repository, IUnitOfWork uow) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(ProductAttributeUpdateCommand command, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.Get(command.Id);

            if (entity is null)
                return await Result.FailAsync("اطلاعات با این مشخصات پیدا نشد.");

            command.Map(entity);

            return await Result.SuccessAsync("اطلاعات با موفقیت ویرایش شد");
        }
    }
}
