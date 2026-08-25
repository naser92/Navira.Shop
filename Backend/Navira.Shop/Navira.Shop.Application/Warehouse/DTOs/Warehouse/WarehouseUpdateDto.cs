using Navira.Shop.Core.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Warehouse
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class WarehouseUpdateDto : BaseDto<int>
    {

        /// <summary>
        ///  Name
        /// </summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        ///  Code
        /// </summary>
        [Display(Name = "Code")]
        public string Code { get; set; }

        /// <summary>
        ///  وضعیت اعتبار
        /// </summary>
        [Display(Name = "وضعیت اعتبار")]
        public bool IsActive { get; set; }

    }
}
