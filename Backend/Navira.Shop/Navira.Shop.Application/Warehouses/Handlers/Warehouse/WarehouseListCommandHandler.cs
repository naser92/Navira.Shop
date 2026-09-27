using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navira.Shop.Application.Warehouses
{
    public class WarehouseListCommandHandler : CommandHandler, IQueryHandler<WarehouseListCommand, object>
    {

        private readonly IWarehouseDapperService _service;

        public WarehouseListCommandHandler(IWarehouseDapperService warehouseService, IUnitOfWork uow) : base(uow)
        {
            _service = warehouseService;
        }

  

        public async Task<IResult<object>> Handle(WarehouseListCommand query, CancellationToken cancellationToken)
        {
            return await _service.GetList(query).ResultAsync();
        }
    }
}
