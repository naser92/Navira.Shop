using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Security;
using Navira.Shop.Core.Web;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Display(Name = "", Description = "")]
    [CustomAuthorize(AuthenticationSchemes = "Bearer")]
    //[Permission("{ControllerName}", AppConsts.SystemBaseInformationName, "{ControllerTitle}")]
    //[Menu(AppConsts.SystemBaseInformationName, AppConsts.SystemBaseInformationTitle)]
    public class ProductImageController : ControllerBase
    {
        #region variables 

        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor 

        public ProductImageController(IBus bus, IQueryBus queryBus)
        {
            _bus = bus;
            _queryBus = queryBus;
        }

        #endregion

        #region Register

        /// <summary>
        /// ثبت اطلاعات  
        /// </summary>
        /// <param name="command">
        /// مشخصات  
        /// </param>
        [HttpPost]
        [Permission("Create", "ایجاد")]
        public virtual async Task<IActionResult> Post(ProductImageRegisterCommand command) =>
             await _bus.Send(command).ApiResultAsync();

        #endregion
    }
}
