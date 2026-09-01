using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Security;
using Navira.Shop.Core.Web;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Display(Name = "پالیسی", Description = "مدیریت پالیسی ها")]
    [CustomAuthorize(AuthenticationSchemes = "Bearer")]
    //[Permission("Controller", "دسترسی", "Identity.Access")]
    //[Menu("Identity.Access", "مدیریت دسترسی")]
    public class PolicyController : Controller
    {
        #region variables 

        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor 


        public PolicyController(IBus bus, IQueryBus queryBus)
        {
            _bus = bus;
            _queryBus = queryBus;
        }

        #endregion

        #region Get main list 

        [HttpGet]
        public virtual async Task<IActionResult> Get() =>
            await _queryBus.Send<PolicyListCommand, object>(new PolicyListCommand()).ApiResultAsync();

        #endregion


        #region Register

        /// <summary>
        /// ثبت اطلاعات  
        /// </summary>
        /// <param name="command">
        /// مشخصات  
        /// </param>
        [HttpPost]
        //[Permission("Create", "ایجاد")]
        public virtual async Task<IActionResult> Post(PolicyRegisterCommand command) =>
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
        //[Permission("Update", "ویرایش")]
        public virtual async Task<IActionResult> Put(PolicyUpdateCommand command) =>
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
        //[Permission("Delete", "حذف")]
        public virtual async Task<IActionResult> Delete(int id) =>
             await _bus.Send(new PolicyDeleteCommand(id)).ApiResultAsync();

        #endregion
    }
}
