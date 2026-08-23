using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Application.Identity;
using Navira.Shop.Application.Identity.Commands.RolePolicy;
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
    public class RolePolicyController
    {


        #region variables 

        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor 


        public RolePolicyController(IBus bus, IQueryBus queryBus)
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
        public virtual async Task<IActionResult> Post(AsingeAndUnAsingRolePolicyCommand command) =>
             await _bus.Send(command).ApiResultAsync();

        #endregion

        #region Get by RoleId


        [HttpGet]
        [Route("{roleId}")]
        //[Permission("Detail", "جزئیات")]
        public virtual async Task<IActionResult> Get(Guid roleId) =>
            await _queryBus.Send<GetRolePolicyByRoleIdCommand, IEnumerable<RolePolicyGetByRoleIdDto>>(new GetRolePolicyByRoleIdCommand { RoleId = roleId }).ApiResultAsync();


        #endregion
    }
}
