using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class CategoryChangeParentCommandHandler : CommandHandler, ICommandHandler<CategoryChangeParentCommand>
    {
        private readonly ICategoryWriteRepository _repository;

        public CategoryChangeParentCommandHandler(IUnitOfWork uow, ICategoryWriteRepository repository) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(CategoryChangeParentCommand command, CancellationToken cancellationToken = default)
        {
            if (command.IsChangeChild)
            {
                var entities = await _repository.GetAll(x => x.Where(i => i.ParentCategoryId == command.SourceCategoryId));
                if (entities.Any())
                {
                    foreach (var entity in entities)
                    {
                        entity.ChangeParentId(command.DesCategoryId);
                    }

                }
                else
                {
                    return Result.Fail("برای این دسته هیچ زیر دسته ای یافت نشد");
                }

            }
            else
            {
                var entity = await _repository.Get(command.SourceCategoryId);
                if (entity != null)
                {
                    entity.ChangeParentId(command.DesCategoryId);
                }
                else
                {
                    return Result.Fail("دسته بندی یافت نشد");
                }

            }

            return Result.Success("تغییرات انجام شد");
        }
    }
}
