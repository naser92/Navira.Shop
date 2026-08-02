using DevExtreme.AspNet.Data;

namespace Navira.Shop.Core.Extensions
{
    public static class GridExtensions
    {
        public static object Grid<T>(this IQueryable<T> queryable, DataSourceLoadOptionsBase parameters)
        {
            return DataSourceLoader.Load(queryable, parameters);
        }

        public static async Task<object> GridAsync<T>(this IQueryable<T> queryable, DataSourceLoadOptionsBase parameters)
        {
            return await DataSourceLoader.LoadAsync(queryable, parameters).ConfigureAwait(false);
        }
        public static object Grid<T>(this IEnumerable<T> list, DataSourceLoadOptionsBase parameters)
        {
            return DataSourceLoader.Load(list, parameters);
        }

    }
}
