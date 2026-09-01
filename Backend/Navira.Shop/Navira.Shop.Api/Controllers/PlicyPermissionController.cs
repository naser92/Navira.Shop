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
    [Display(Name = "پالیسی", Description = "مدیریت ارتباط نقش و سیاست ها")]
    [CustomAuthorize(AuthenticationSchemes = "Bearer")]
    //[Permission("Controller", "دسترسی", "Identity.Access")]
    //[Menu("Identity.Access", "مدیریت دسترسی")]
    public class PlicyPermissionController : Controller
    {
        #region variables 

        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor 


        public PlicyPermissionController(IBus bus, IQueryBus queryBus)
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
        //[Permission("Create", "ایجاد")]
        public virtual async Task<IActionResult> Post(AsingeAndUnAsingPolicyPermissionCommand command) =>
             await _bus.Send(command).ApiResultAsync();

        #endregion

        #region Get by PolicyId


        [HttpGet]
        [Route("{policyId}")]
        //[Permission("Detail", "جزئیات")]
        public virtual async Task<IActionResult> Get(int policyId) =>
            await _queryBus.Send<GetPolicyPermissionByPolicyIdCommand, IEnumerable<PolicyPermissionGetByPolicyIdDto>>(new GetPolicyPermissionByPolicyIdCommand { PolicyId = policyId }).ApiResultAsync();


        #endregion
    }
}
