using Navira.Shop.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navira.Shop.Application.Warehouses
{
    public class WarehouseListFilterDto : PaginationModel
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public bool? IsActive { get; set; }
    }
}
