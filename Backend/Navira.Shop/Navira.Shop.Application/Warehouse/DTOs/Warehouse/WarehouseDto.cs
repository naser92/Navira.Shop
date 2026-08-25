using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.ApplicationService.Dto
{

 
    public class WarehouseDto : BaseDto<int>
    {

        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

    }
}
