using DocumentFormat.OpenXml.Office2010.Excel;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navira.Shop.Application.Warehouses
{
    public class WarehouseDeleteCommand : BaseDto<int>, ICommand
    {

        public WarehouseDeleteCommand(int id)
        {
            Id = id;
        }

    }
}
