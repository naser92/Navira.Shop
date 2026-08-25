using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Application.Catalog
{

    /// <summary>
    ///  
    /// </summary>
    [Display(Name = "", Description = "")]
    public class ProductAttributeRegisterDto
    {


        /// <summary>
        ///  Name
        /// </summary>
        [Display(Name = "Name")]
        public string Name { get; set; }


        /// <summary>
        ///  ValueType
        /// </summary>
        [Display(Name = "ValueType")]
        public string ValueType { get; set; }


        /// <summary>
        ///  Usage
        /// </summary>
        [Display(Name = "Usage")]
        public string Usage { get; set; }


        /// <summary>
        ///  وضعیت اعتبار
        /// </summary>
        [Display(Name = "وضعیت اعتبار")]
        public bool IsActive { get; set; }

    }
}
