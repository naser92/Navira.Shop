using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Infrastructure;
using Navira.Shop.Core.Ioc;
using System.Data;

namespace Navira.Shop.Core.Dapper
{
    public class DependencyConfigurator : IDependencyRegistrar
    {
        public int Order => 0;

        public void Register(IServiceCollection serviceCollection, ITypeFinder typeFinder, AppSettings appSettings)
        {
            foreach (var ConnectionString in appSettings.ConnectionStrings)
            {

                serviceCollection.AddKeyedTransient<IDbConnection>(ConnectionString.Key, (sp, key) => new SqlConnection(ConnectionString.Value));
            }

        }

    }
}
