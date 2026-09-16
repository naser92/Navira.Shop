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
    [Permission("Controller", "موارد ویژگی", "catalog.ProductAttribute")]
    [Menu("catalog.product", "مدیریت موارد ویژگی")]
    public class ProductAttributeOptionController : ControllerBase
    {
        #region variables 

        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor 

        public ProductAttributeOptionController(IBus bus, IQueryBus queryBus)
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
        [Permission("List", "لیست موارد ویژگی ها")]
        public virtual async Task<IActionResult> Get([FromQuery] ProductAttributeOptionListCommand command) =>
             await _queryBus.Send<ProductAttributeOptionListCommand, object>(command).ApiResultAsync();

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
        public virtual async Task<IActionResult> Post(ProductAttributeOptionRegisterCommand command) =>
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
        public virtual async Task<IActionResult> Put(ProductAttributeOptionUpdateCommand command) =>
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
             await _bus.Send(new ProductAttributeOptionDeleteCommand(id)).ApiResultAsync();

        #endregion
    }
}
