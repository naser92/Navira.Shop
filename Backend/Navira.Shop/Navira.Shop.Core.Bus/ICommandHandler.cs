using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Core.Bus
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        IUnitOfWork Uow { get; }

        Task<IResult> Handle(TCommand command, CancellationToken cancellationToken = default);
    }

    public interface IQueryHandler<TQuery, TQueryResult> where TQuery : ICommand
    {
        Task<IResult<TQueryResult>> Handle(TQuery query, CancellationToken cancellationToken = default);
    }

    public abstract class CommandHandler
    {
        public IUnitOfWork Uow { get; private set; }
        public CommandHandler(IUnitOfWork uow)
        {
            Uow = uow;
        }
    }
}
