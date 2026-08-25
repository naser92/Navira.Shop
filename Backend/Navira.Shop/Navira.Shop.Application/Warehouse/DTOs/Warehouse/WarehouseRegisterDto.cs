using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.ApplicationService.Dto
{
    
    /// <summary>
    ///  
    /// </summary>
    [Display(Name="",Description="")]
    public class WarehouseRegisterDto
    {


        /// <summary>
        ///  Name
        /// </summary>
        [Display(Name="Name")]
        public string Name { get; set; }


        /// <summary>
        ///  Code
        /// </summary>
        [Display(Name="Code")]
        public string Code { get; set; }


        /// <summary>
        ///  وضعیت اعتبار
        /// </summary>
        [Display(Name="وضعیت اعتبار")]
        public bool IsActive { get; set; }

    }
}
