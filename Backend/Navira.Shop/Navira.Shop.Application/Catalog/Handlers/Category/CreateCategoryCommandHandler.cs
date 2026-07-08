using Navira.Shop.Application.Catalog.Commands;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Domain;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog.Entities;

namespace Navira.Shop.Application.Handlers
{
    public class CreateCategoryCommandHandler : CommandHandler, ICommandHandler<CreateCategoryCommand>
    {
        private readonly IAggregateRepository<Category, int> _categories;
        public CreateCategoryCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public Task<IResult> Handle(CreateCategoryCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
