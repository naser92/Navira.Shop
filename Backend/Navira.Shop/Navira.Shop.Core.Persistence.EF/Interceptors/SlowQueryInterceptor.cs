using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Navira.Shop.Core.Configuration;
using NaviraShop.Core.Mq;
using System.Data.Common;

namespace Navira.Shop.Core.Persistence.EF
{
    public class SlowQueryInterceptor : DbCommandInterceptor
    {
        private readonly TimeSpan _threshold = TimeSpan.FromMilliseconds(1000);
        private readonly IPublisher _publisher;
        private readonly AppSettings _appsetting;

        public SlowQueryInterceptor(IPublisher publisher, AppSettings appSettings)
        {
            _publisher = publisher;
            _appsetting = appSettings;
        }


        public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
        {
            if (eventData.Duration.TotalMilliseconds > _threshold.TotalMilliseconds)
                _publisher.Log("Diagnostics : Slow NonQuery (EF)", new(command?.Connection?.Database, command.CommandText, eventData.Duration), LogLevel.Warning);
            return base.NonQueryExecuted(command, eventData, result);

        }

        public override ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds > _threshold.TotalMilliseconds)
                _publisher.Log("Diagnostics : Slow NonQuery (EF)", new(command?.Connection?.Database, command.CommandText, eventData.Duration), LogLevel.Warning);
            return base.NonQueryExecutedAsync(command, eventData, result);
        }



        public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        {

            if (eventData.Duration.TotalMilliseconds > _threshold.TotalMilliseconds)
                _publisher.Log("Diagnostics : Slow Query (EF)", new(command?.Connection?.Database, command.CommandText, eventData.Duration), LogLevel.Warning);
            return base.ReaderExecuted(command, eventData, result);

        }

        public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {

            if (eventData.Duration.TotalMilliseconds > _threshold.TotalMilliseconds)
                _publisher.Log("Diagnostics : Slow Query (EF)", new(command?.Connection?.Database, command.CommandText, eventData.Duration), LogLevel.Warning);
            return base.ReaderExecutedAsync(command, eventData, result);
        }



    }
}