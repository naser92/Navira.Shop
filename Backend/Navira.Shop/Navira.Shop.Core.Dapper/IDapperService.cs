using Navira.Shop.Core.Service;
using System.Data;
using static Dapper.SqlMapper;

namespace Navira.Shop.Core.Dapper
{
    public interface IDapperService : IBaseService
    {
        Task Execute(string SqlQuery, object paramentes = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure);
        Task<T> ExecuteScalar<T>(string SqlQuery, object paramentes = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure);

        Task<IEnumerable<T>> Query<T>(string SqlQuery, object paramentes = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure);
        Task<IEnumerable<TR>> Query<T1, T2, TR>(string SqlQuery, Func<T1, T2, TR> mpp, object paramentes = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = CommandType.StoredProcedure);
        Task<IEnumerable<TR>> Query<T1, T2, T3, TR>(string SqlQuery, Func<T1, T2, T3, TR> mpp, object paramentes = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = CommandType.StoredProcedure);

        //Task<object> Query<T>(string SqlQuery, GridParameters gridParameters, DynamicParameters paramentes = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure);

        Task<T> SingleOrDefault<T>(string SqlQuery, object paramentes, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure);
        Task<T> FirstOrDefault<T>(string SqlQuery, object paramentes, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure);

        Task<T> QueryMultiple<T>(string SqlQuery, object paramentes = null, Func<GridReader, T> mapper = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure);
        Task<T> QueryMultiple<T>(string SqlQuery, object paramentes = null, Func<GridReader, Task<T>> mapper = null, IDbTransaction transaction = null, CommandType? commandType = CommandType.StoredProcedure);
    }
}
