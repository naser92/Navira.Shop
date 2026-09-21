using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Security;
using Navira.Shop.Core.Web;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Display(Name = "دسته ها", Description = "مدیریت دسته ها")]
    [CustomAuthorize(AuthenticationSchemes = "Bearer")]
    [Permission("Controller", "دسته بندی ها", "catalog.Category")]
    [Menu("catalog.product", "مدیریت محصول")]
    public class CategoryAdminController : ControllerBase
    {

        #region variables 

        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor 

        public CategoryAdminController(IBus bus, IQueryBus queryBus)
        {
            _bus = bus;
            _queryBus = queryBus;
        }

        #endregion


        #region Get main list 

        /// <summary>
        /// لیست  
        /// </summary>
        /// <param name = "parameters" >
        /// پارامتر های سفارشی سازی لیست
        /// </param>
        [HttpGet]
        [Permission("List", "لیست دسته بندی ها")]
        [Menu("CategoryAdmin.List", "دسته بندی ها", Action = "Category")]
        public virtual async Task<IActionResult> Get([FromQuery] CategoryListCommand commansd) =>
            await _queryBus.Send<CategoryListCommand, object>(commansd).ApiResultAsync();


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
        public virtual async Task<IActionResult> Post(CategoryRegisterCommand command) =>
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
        public virtual async Task<IActionResult> Put(CategoryUpdateCommand command) =>
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
             await _bus.Send(new CategoryDeleteCommand(id)).ApiResultAsync();


        #endregion

        #region ChangeParentId
        [HttpPost]
        [Route("ChangeParent")]
        [Permission("ChangeParent", "تعویض والد")]
        public virtual async Task<IActionResult> ChangeParent([FromBody] CategoryChangeParentCommand command) =>
             await _bus.Send(command).ApiResultAsync();
        #endregion
    }
}
