using Navira.Shop.Core.Results;

namespace Navira.Shop.Core.Bus
{
    public interface IBus
    {
        Task<IResult> Send<TCommand>(TCommand command) where TCommand : ICommand;
    }

    public interface IQueryBus
    {
        Task<IResult<TQueryResult>> Send<TQuery, TQueryResult>(TQuery query) where TQuery : ICommand;
    }

}
