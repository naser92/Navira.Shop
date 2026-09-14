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
    [Permission("Controller", "ویژگی های محصول", "catalog.ProductAttribute")]
    [Menu("catalog.product", "مدیریت محصول")]
    public class ProductAttributeController : ControllerBase
    {
        #region variables 

        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor 

        public ProductAttributeController(IBus bus, IQueryBus queryBus)
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
        public virtual async Task<IActionResult> Post(ProductAttributeRegisterCommand command) =>
             await _bus.Send(command).ApiResultAsync();

        #endregion

        #region Get main list 

        /// <summary>
        /// لیست  
        /// </summary>
        /// <param name="parameters">
        /// پارامتر های سفارشی سازی لیست
        /// </param>
        [HttpGet]
        //[Permission("List", "{ControllerName}", "{ ControllerTitle}")]
        //[Menu("{ControllerName}", "{ControllerTitle}", "List")]
        public virtual async Task<IActionResult> Get([FromQuery] ProductAttributeListCommand parameters) =>
                 await _queryBus.Send<ProductAttributeListCommand, object>(parameters).ApiResultAsync();

        #endregion
    }
}
