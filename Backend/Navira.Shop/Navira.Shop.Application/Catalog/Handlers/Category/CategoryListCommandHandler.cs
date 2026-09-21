using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Catalog
{
    public class CategoryListCommandHandler : CommandHandler, IQueryHandler<CategoryListCommand, object>
    {

        private readonly ICategoryQueryService _service;

        public CategoryListCommandHandler(ICategoryQueryService categoryService, IUnitOfWork uow) : base(uow)
        {
            _service = categoryService;
        }

        public async Task<IResult<object>> Handle(CategoryListCommand query, CancellationToken cancellationToken = default)
        {
            return await _service.GetSort().ResultAsync();
        }

    }
}
