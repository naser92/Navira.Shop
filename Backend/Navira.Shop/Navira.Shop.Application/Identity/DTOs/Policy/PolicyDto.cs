using Navira.Shop.Core.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Identity
{
    public class PolicyDto : BaseDto<int>
    {
        /// <summary>
        ///  Name
        /// </summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

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
        ///  IsSystem
        /// </summary>
        [Display(Name = "IsSystem")]
        public bool IsSystem { get; set; }

        /// <summary>
        ///  وضعیت اعتبار
        /// </summary>
        [Display(Name = "وضعیت اعتبار")]
        public bool IsActive { get; set; }
    }
}
