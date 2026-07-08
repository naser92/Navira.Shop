using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Infrastructure;
using Navira.Shop.Core.Persistence.EF;
using NaviraShop.Core.Mq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class WriteDbContext  : WriteFrameworkDbContext
    {
        public WriteDbContext() : base()
        {
        }

        public WriteDbContext(DbContextOptions<WriteDbContext> options, IBus bus, IPublisher publisher) : base(
            options, bus, publisher)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseBuyerTypeMapping).Assembly);
        //    base.OnModelCreating(modelBuilder);
        //}
    }

    public class WriteDbContextFactory : IDesignTimeDbContextFactory<WriteDbContext>
    {
        private ITypeFinder _typeFinder;
        private readonly DbContextOptions<WriteDbContext> _options;

        public WriteDbContextFactory()
        {
            var connectionString = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory().Replace("Persistence", "Api"))
                .AddJsonFile("appsettings.Development.json")
                .Build().GetConnectionString("WriteConnection");
            _options = new DbContextOptionsBuilder<WriteDbContext>()
                .UseSqlServer(connectionString, k => k.UseHierarchyId()).Options;
        }

        public WriteDbContextFactory(DbContextOptions<WriteDbContext> options)
        {
            _options = options;
        }

        public WriteDbContextFactory(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }

        public WriteDbContext CreateDbContext(string[] args)
        {
            return new WriteDbContext(_options, default, null);
        }
    }
}
}
