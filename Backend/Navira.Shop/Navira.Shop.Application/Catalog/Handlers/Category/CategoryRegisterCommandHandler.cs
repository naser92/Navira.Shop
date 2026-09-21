using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class CategoryRegisterCommandHandler : CommandHandler, ICommandHandler<CategoryRegisterCommand>
    {
        private readonly ICategoryWriteRepository _repository;
        private readonly ICategoryQueryService _queryService;
        public CategoryRegisterCommandHandler(
            ICategoryWriteRepository repository, IUnitOfWork uow, ICategoryQueryService queryService) : base(uow)
        {
            _repository = repository;
            _queryService = queryService;
        }

        public async Task<IResult> Handle(CategoryRegisterCommand command, CancellationToken cancellationToken = default)
        {
            if (await _queryService.IsExisitName(command.Name))
                return await Result.FailAsync($"نام دسته وجود دارد");

            var entiy = command.Map<Category>();

            await _repository.Insert(entiy);
            return await Result.SuccessAsync("اطلاعات با موفقیت ثبت شد");
        }
    }
}
