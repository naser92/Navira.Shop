using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog.Handlers.Category
{
    public class CategoryUpdateCommandHandler : CommandHandler, ICommandHandler<CategoryUpdateCommand>
    {
        private readonly ICategoryWriteRepository _repository;
        public CategoryUpdateCommandHandler(IUnitOfWork uow, ICategoryWriteRepository repository) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(CategoryUpdateCommand command, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.Get(command.Id);

            if (entity is null)
                return await Result.FailAsync("اطلاعات با این مشخصات پیدا نشد.");

            command.Map(entity);

            return await Result.SuccessAsync("اطلاعات با موفقیت ویرایش شد");
        }
    }
}
