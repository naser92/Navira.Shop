using Microsoft.EntityFrameworkCore;
using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Infrastructure.Identity.Mappers.ReadModel;
using NaviraShop.Core.Mq;

namespace Navira.Shop.Infrastructure.Persistence
{
    public class QueryDbContext : QueryFrameworkDbContext
    {
        public QueryDbContext(DbContextOptions options, IPublisher publisher) : base(options, publisher)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MenuModelMapper).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
