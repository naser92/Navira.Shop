using Dapper;
using Microsoft.Extensions.Logging;
using NaviraShop.Core.Mq;
using System.Data;
using System.Diagnostics;
using static Dapper.SqlMapper;

namespace Navira.Shop.Core.Dapper
{
    public abstract class DapperBaseService : IDapperService
    {
        protected readonly IDbConnection _dbConnection;
        private readonly IPublisher _publisher;
        private readonly TimeSpan _threshold = TimeSpan.FromMilliseconds(1000);
        private DapperServiceOptions _options;
        private readonly int commandTimeout;
        private string _databaseName;
        public DapperBaseService(IDbConnection dbConnection, IPublisher publisher)
        {
            _dbConnection = dbConnection;
            _publisher = publisher;
            commandTimeout = 300;
        }

        private async Task Run(Func<string, object, IDbTransaction, CommandType?, Task> func,
           string SqlQuery, object paramentes = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await func(SqlQuery, paramentes, transaction, commandType);
            }
            finally
            {
                stopwatch.Stop();
                if (stopwatch.Elapsed.TotalMilliseconds > _threshold.TotalMilliseconds)
                    await _publisher.Log("Diagnostics(Dapper) : Slow Query", new(_databaseName, SqlQuery, stopwatch.Elapsed), LogLevel.Warning);
            }
        }
        private async Task<TResult> Run<TResult>(Func<string, object, IDbTransaction, CommandType?, Task<TResult>> func,
            string SqlQuery, object paramentes, IDbTransaction transaction, CommandType? commandType)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                return await func(SqlQuery, paramentes, transaction, commandType);
            }
            finally
            {
                stopwatch.Stop();
                if (stopwatch.Elapsed.TotalMilliseconds > _threshold.TotalMilliseconds)
                    await _publisher.Log("Diagnostics(Dapper) : Slow Query", new(_databaseName, SqlQuery, stopwatch.Elapsed), LogLevel.Warning);
            }
        }

        public async Task Execute(string SqlQuery, object paramentes = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure)
        {
            await Run(async (string q, object p, IDbTransaction t, CommandType? c) =>
            {
                await _dbConnection.ExecuteAsync(q, p, t, commandTimeout, c);
            }, SqlQuery, paramentes, transaction, commandType);


        }
        public async Task<T> ExecuteScalar<T>(string SqlQuery, object paramentes = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure)
        {

            return await Run(async (string q, object p, IDbTransaction t, CommandType? c) =>
            {
                return await _dbConnection.ExecuteScalarAsync<T>(q, p, t, commandTimeout, c);
            }, SqlQuery, paramentes, transaction, commandType);

        }
        public async Task<T> QueryMultiple<T>(string SqlQuery, object paramentes = null, Func<GridReader, T> mapper = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure)
        {
            if (mapper == null)
                return default(T);
            return await Run(async (string q, object p, IDbTransaction t, CommandType? c) =>
            {
                using (var data = await _dbConnection.QueryMultipleAsync(q, p, t, commandTimeout, c))
                {
                    return mapper(data);
                }
            }, SqlQuery, paramentes, transaction, commandType);

        }
        public async Task<T> QueryMultiple<T>(string SqlQuery, object paramentes = null, Func<GridReader, Task<T>> mapper = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure)
        {
            if (mapper == null)
                return default(T);
            return await Run(async (string q, object p, IDbTransaction t, CommandType? c) =>
            {
                using (var data = await _dbConnection.QueryMultipleAsync(q, p, t, commandTimeout, c))
                {
                    return await mapper(data);
                }
            }, SqlQuery, paramentes, transaction, commandType);

        }

        public async Task<IEnumerable<T>> Query<T>(string SqlQuery, object paramentes = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure)
        {
            return await Run(async (string q, object p, IDbTransaction t, CommandType? c) =>
            {
                return await _dbConnection.QueryAsync<T>(q, p, t, commandTimeout, c);
            }, SqlQuery, paramentes, transaction, commandType);
        }
        public async Task<IEnumerable<TR>> Query<T1, T2, TR>(string SqlQuery, Func<T1, T2, TR> map, object paramentes = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = CommandType.StoredProcedure)
        {
            return await Run(async (string q, object p, IDbTransaction t, CommandType? c) =>
            {
                return await _dbConnection.QueryAsync<T1, T2, TR>(SqlQuery, map, paramentes, transaction, buffered, splitOn, commandTimeout, commandType);
            }, SqlQuery, paramentes, transaction, commandType);
        }
        public async Task<IEnumerable<TR>> Query<T1, T2, T3, TR>(string SqlQuery, Func<T1, T2, T3, TR> map, object paramentes = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = CommandType.StoredProcedure)
        {
            return await Run(async (string q, object p, IDbTransaction t, CommandType? c) =>
            {
                return await _dbConnection.QueryAsync<T1, T2, T3, TR>(SqlQuery, map, paramentes, transaction, buffered, splitOn, commandTimeout, commandType);
            }, SqlQuery, paramentes, transaction, commandType);
        }
        //public async Task<object> Query<T>(string SqlQuery, GridParameters gridParameters, DynamicParameters paramentes = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure)
        //{
        //    return await Run(async (string q, object p, IDbTransaction t, CommandType? c) =>
        //    {
        //        var data = await _dbConnection.QueryAsync<T>(q, p, t, commandTimeout, c);
        //        var count = await _dbConnection.ExecuteScalarAsync<int>(q, p, t, commandTimeout, c);

        //        return data.ToGrid(count);
        //    }, SqlQuery, paramentes, transaction, commandType);
        //}


        public async Task<T> SingleOrDefault<T>(string SqlQuery, object paramentes, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure)
        {
            return await Run(async (string q, object p, IDbTransaction t, CommandType? c) =>
            {
                return await _dbConnection.QuerySingleOrDefaultAsync<T>(q, p, t, commandTimeout, c);

            }, SqlQuery, paramentes, transaction, commandType);
        }
        public async Task<T> FirstOrDefault<T>(string SqlQuery, object paramentes, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure)
        {

            return await Run(async (string q, object p, IDbTransaction t, CommandType? c) =>
            {
                return await _dbConnection.QueryFirstOrDefaultAsync<T>(q, p, t, commandTimeout, c);
            }, SqlQuery, paramentes, transaction, commandType);

        }
    }
}
