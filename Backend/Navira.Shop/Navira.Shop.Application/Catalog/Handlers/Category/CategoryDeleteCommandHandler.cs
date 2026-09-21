using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class CategoryDeleteCommandHandler : CommandHandler, ICommandHandler<CategoryDeleteCommand>
    {
        private readonly ICategoryWriteRepository _repository;

        public CategoryDeleteCommandHandler(ICategoryWriteRepository repository, IUnitOfWork uow) : base(uow)
        {

            _repository = repository;

        }

        public async Task<IResult> Handle(CategoryDeleteCommand command, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.Get(command.Id);

            if (entity is null)
                return await Result.FailAsync("اطلاعات با این مشخصات پیدا نشد.");

            await _repository.Delete(entity);

            return await Result.SuccessAsync("اطلاعات با موفقیت حذف شد");
        }
    }
}
