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
    [Permission("Controller", "برندها", "catalog.Brand")]
    [Menu("catalog.product", "مدیریت محصول")]
    public class BrandAdminController : ControllerBase
    {
        #region variables 

        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor 

        public BrandAdminController(IBus bus, IQueryBus queryBus)
        {
            _bus = bus;
            _queryBus = queryBus;
        }

        #endregion

        #region Get main list 

        /// <summary>
        /// لیست  
        /// </summary>
        /// <param name="parameters">
        /// پارامتر های سفارشی سازی لیست
        /// </param>
        [HttpGet]
        [Permission("List", "لیست برندها")]
        [Menu("BrandAdmin.List", "برندها", Action = "brand")]
        public virtual async Task<IActionResult> Get([FromQuery] BrandListCommand parameters) =>
            await _queryBus.Send<BrandListCommand, object>(parameters).ApiResultAsync();

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
        public virtual async Task<IActionResult> Post([FromBody] BrandRegisterCommand command) =>
             await _bus.Send(command).ApiResultAsync();


        #endregion

        #region Edit

        /// <summary>
        /// ویرایش  
        /// </summary>
        /// <param name="command">
        /// مشخصات  
        /// </param>
        [HttpPut]
        [Permission("Update", "ویرایش")]
        public virtual async Task<IActionResult> Put(BrandUpdateCommand command) =>
            await _bus.Send(command).ApiResultAsync();

        #endregion

        #region Delete

        /// <summary>
        /// حذف  
        /// </summary>
        /// <param name="id">
        /// شناسه  
        /// </param>
        [HttpDelete]
        [Route("{id}")]
        [Permission("Delete", "حذف")]
        public virtual async Task<IActionResult> Delete(int id) =>
            await _bus.Send(new BrandDeleteCommand(id)).ApiResultAsync();

        #endregion

    }
}
