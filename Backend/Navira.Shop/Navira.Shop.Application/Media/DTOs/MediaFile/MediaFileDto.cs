using Navira.Shop.Core.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Media
{
    public class MediaFileDto : BaseDto<int>
    {

        /// <summary>
        ///  ProcessingProfile
        /// </summary>
        [Display(Name = "ProcessingProfile")]
        public string ProcessingProfile { get; set; }

        /// <summary>
        ///  ProcessingProfileVersion
        /// </summary>
        [Display(Name = "ProcessingProfileVersion")]
        public int ProcessingProfileVersion { get; set; }

        /// <summary>
        ///  EditSettingsJson
        /// </summary>
        [Display(Name = "EditSettingsJson")]
        public string EditSettingsJson { get; set; }
    }

}
