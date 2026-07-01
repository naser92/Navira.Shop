using Microsoft.EntityFrameworkCore;
using System;

namespace Navira.Shop.Core.Persistence.EF
{
    public interface IEntityMapper
    {
        Type MapperType { get; }
        void Map(ModelBuilder modelBuilder);
    }
}
