using Navira.Shop.Core.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Identity
{
    public class PermissionModelDto : BaseDto<int>
    {

        /// <summary>
        ///  BaseSubSystemId
        /// </summary>
        [Display(Name = "BaseSubSystemId")]
        public int BaseSubSystemId { get; set; }

        /// <summary>
        ///  ControllerName
        /// </summary>
        [Display(Name = "ControllerName")]
        public string ControllerName { get; set; }

        /// <summary>
        ///  Scope
        /// </summary>
        [Display(Name = "Scope")]
        public string Scope { get; set; }

        /// <summary>
        ///  Code
        /// </summary>
        [Display(Name = "Code")]
        public string Code { get; set; }

        /// <summary>
        ///  Title
        /// </summary>
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        ///  Description
        /// </summary>
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        ///  وضعیت اعتبار
        /// </summary>
        [Display(Name = "وضعیت اعتبار")]
        public bool IsActive { get; set; }
    }
}
