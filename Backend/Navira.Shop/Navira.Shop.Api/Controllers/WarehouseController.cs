using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Security;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Display(Name = "", Description = "")]
    [CustomAuthorize(AuthenticationSchemes = "Bearer")]
    //[Permission("{ControllerName}", AppConsts.SystemBaseInformationName, "{ControllerTitle}")]
    //[Menu(AppConsts.SystemBaseInformationName, AppConsts.SystemBaseInformationTitle)]
    public class WarehouseController
    {
        #region variables 

        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor 

        public WarehouseController(IBus bus, IQueryBus queryBus)
        {
            _bus = bus;
            _queryBus = queryBus;
        }

        #endregion
        //#region Get main list 

        ///// <summary>
        ///// لیست  
        ///// </summary>
        ///// <param name="parameters">
        ///// پارامتر های سفارشی سازی لیست
        ///// </param>
        //[HttpGet]
        ////[Permission("List", "{ControllerName}", "{ ControllerTitle}")]
        ////[Menu("{ControllerName}", "{ControllerTitle}", "List")]
        //public virtual async Task<IActionResult> Get([FromQuery] WarehouseListCommandHandler parameters) =>
        //    await _queryBus.Send<WarehouseListCommandHandler, object>(parameters).ApiResultAsync();

        //#endregion
    }
}
