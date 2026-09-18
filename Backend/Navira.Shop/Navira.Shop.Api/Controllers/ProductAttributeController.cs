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
        [Permission("List", "لیست ویژگی ها")]
        [Menu("ProductAttribute.List", "ویژگی ها", Action = "ProductAttribute")]
        public virtual async Task<IActionResult> Get([FromQuery] ProductAttributeListCommand parameters) =>
                 await _queryBus.Send<ProductAttributeListCommand, object>(parameters).ApiResultAsync();

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
        public virtual async Task<IActionResult> Put(ProductAttributeUpdateCommand command) =>
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
             await _bus.Send(new ProductAttributeDeleteCommand(id)).ApiResultAsync();


        #endregion


        #region Get by Id

        /// <summary>
        /// دریافت جزئیات  
        /// </summary>
        /// <param name="id">
        /// شناسه  
        /// </param>
        [HttpGet]
        [Route("{id}")]
        [Permission("Detail", "جزئیات")]
        public virtual async Task<IActionResult> Get(int id) =>
             await _queryBus.Send<ProductAttributeDetailCommand, ProductAttributeDto>(new ProductAttributeDetailCommand(id)).ApiResultAsync();

        #endregion

    }
}
