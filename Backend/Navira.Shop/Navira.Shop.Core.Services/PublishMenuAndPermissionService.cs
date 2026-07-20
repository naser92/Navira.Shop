using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Infrastructure;
using Navira.Shop.Core.Security;
using Navira.Shop.Core.ViewModels;
using NaviraShop.Core.Mq;
using SmartFormat;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;

namespace Navira.Shop.Core.Services
{
    public class PublishMenuAndPermissionService : IPublishMenuAndPermissionService
    {
        //private PermissionDto _permission;
        //private List<MenuDto> _menus;
        private readonly IKeycloakAdminClientService _keycloak;
        private readonly IPublisher _publisher;
        private readonly ITypeFinder _typeFinder;
        private int _subSystemName;
        private readonly AppSettings _appSettings;

        public PublishMenuAndPermissionService(IPublisher publisher, ITypeFinder typeFinder, AppSettings appSettings, IKeycloakAdminClientService keycloak)
        {
            _publisher = publisher;
            _typeFinder = typeFinder;
            _appSettings = appSettings;
            _keycloak = keycloak;
        }
        public async Task BuildPermissionAndMenu(params string[] controllerNames)
        {


            _subsystemId = _appSettings.SystemInfo.Id;
            var permissions = Scan(controllerNames);

            foreach (var item in permissions)
            {
                //1- Create Resource
                var resourceId = await _keycloak.EnsureResourceAsync(item.Resource);


                //2- Create Scope
                await _keycloak.EnsureScopeAsync(item.Scope);

                //3- Attach Scope

                await _keycloak.AttachScopeToResourceAsync(resourceId,item.Scope);
            }
        }

     


        private  IReadOnlyList<PermissionDto> Scan(params string[] controllerNames)
        {
            var result = new List<PermissionDto>();
            var controllers = _typeFinder.FindClassesOfType<Controller>();

            if (controllerNames.IsNotNullOrEmpty())
                controllers = controllers.Where(x => controllerNames.Any(y => y == x.Name));

            if (controllers.IsNotNullOrEmpty())
            {
                foreach (var controller in controllers)
                {
                    var display = controller.GetCustomAttribute<DisplayAttribute>();
                    var controllerTitle = display.IsNull() ? "" : display.Name;
                    var controllerName = controller.Name.Replace("Controller", "");

                    var methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var method in methods)
                    {

                        var attribute = method.GetCustomAttribute<PermissionAttribute>();

                        if (attribute == null)
                            continue;

                        result.Add(new PermissionDto()
                        {
                            Scope = attribute.Scope ?? method.Name,
                            Resource = controllerName,
                            Route = GetRoute(controller, method),
                            HttpMethod = GetHttpMethod(method)
                        });


                    }
                }
            }

            return result;
        }

        private string? GetRoute(Type controller, MethodInfo method)
        {

            var controllerRoute = controller.GetCustomAttribute<RouteAttribute>()?.Template;


            var methodRoute = method.GetCustomAttribute<RouteAttribute>()?.Template;


            return $"{controllerRoute}/{methodRoute}";
        }

        private string? GetHttpMethod(MethodInfo method)
        {

            if (method.GetCustomAttribute<HttpGetAttribute>() != null)
                return "GET";


            if (method.GetCustomAttribute<HttpPostAttribute>() != null)
                return "POST";


            if (method.GetCustomAttribute<HttpPutAttribute>() != null)
                return "PUT";


            if (method.GetCustomAttribute<HttpDeleteAttribute>() != null)
                return "DELETE";


            return null;
        }
    }
}

