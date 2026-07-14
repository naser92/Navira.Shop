using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Core.Bus
{
    public class BusControl : IBus, IQueryBus
    {
        private readonly IServiceProvider _serviceProvider;
        public BusControl(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }



        public async Task<IResult> Send<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
        {
            await ValidateAsync(command).ConfigureAwait(false);

            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            try
            {

                var result = await handler.Handle(command);
                await handler.Uow.Commit();

                return result;
            }
            catch (Exception ex)
            {
                await handler.Uow.Rollback();
                throw ex;
            }

        }
        private async Task ValidateAsync<TCommand>(TCommand request)
        {
            var validator = _serviceProvider.GetService<AbstractValidator<TCommand>>();
            if (validator is null)
                return;

            var validation = await validator.ValidateAsync(request).ConfigureAwait(false);

            if (!validation.IsValid)
                throw new ResultException(validation.ToString());
            return;
        }


        public async Task<IResult<TQueryResult>> Send<TQuery, TQueryResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : ICommand
        {
            await ValidateAsync(query).ConfigureAwait(false);

            var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();

            return await handler.Handle(query);
        }


    }
}
