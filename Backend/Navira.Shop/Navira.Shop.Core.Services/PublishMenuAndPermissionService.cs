using ClosedXML;
using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Infrastructure;
using Navira.Shop.Core.Security;
using Navira.Shop.Core.ViewModels;
using NaviraShop.Core.Mq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Navira.Shop.Core.Services
{
    public class PublishMenuAndPermissionService : IPublishMenuAndPermissionService
    {
        //private PermissionDto _permission;
        //private List<MenuDto> _menus;

        private readonly IPublisher _publisher;
        private readonly ITypeFinder _typeFinder;
        private int _subsystemId;
        private readonly AppSettings _appSettings;
        private List<MenuDto> _menus;
        private List<PermissionDto> _permissions;

        public PublishMenuAndPermissionService(IPublisher publisher, ITypeFinder typeFinder, AppSettings appSettings)
        {
            _publisher = publisher;
            _typeFinder = typeFinder;
            _appSettings = appSettings;
        }
        public async Task<RegisterMenuAndPermissionEvent> BuildPermissionAndMenu(params string[] controllerNames)
        {


            _subsystemId = 1;// _appSettings.SystemInfo.Id;
            var controllers = _typeFinder.FindClassesOfType<ControllerBase>();

            if (controllerNames.IsNotNullOrEmpty())
                controllers = controllers.Where(x => controllerNames.Any(y => y == x.Name));
            var result = new RegisterMenuAndPermissionEvent();
            _menus = new List<MenuDto>();
            _permissions = new List<PermissionDto>();
            if (controllers.IsNotNullOrEmpty())
            {
                foreach (var controller in controllers)
                {
                    var display = controller.GetCustomAttribute<DisplayAttribute>();
                    var controllerTitle = display.IsNull() ? "" : display.Name;
                    var controllerName = controller.Name.Replace("Controller", "");

                    var permissionAttribute = controller.GetCustomAttribute<PermissionAttribute>();
                    var menuAttribute = controller.GetCustomAttribute<MenuAttribute>();

                    if (!permissionAttribute.IsNull())
                    {
                        _permissions.Add(new PermissionDto
                        {
                            BaseSubSystemId = _subsystemId,
                            ControllerName = controllerName,
                            Scope = "Controller",
                            Code = permissionAttribute.Code.IsNullOrWhiteSpace() ? $"{controllerName}.Controller" : permissionAttribute.Code,
                            Title = permissionAttribute.Title,
                        });
                        controller.GetMethods().Where(w => w.HasAttribute<PermissionAttribute>()).ToList().ForEach(
                            action =>
                            {
                                var actionPermission = action.GetCustomAttribute<PermissionAttribute>();
                                _permissions.Add(new PermissionDto
                                {
                                    BaseSubSystemId = _subsystemId,
                                    ControllerName = controllerName,
                                    Scope = actionPermission.Scope,
                                    Code = $"{controllerName}.{actionPermission.Scope}",
                                    Title = actionPermission.Title,
                                });
                            }
                            );
                    }

                    if (!menuAttribute.IsNull())
                    {
                        var parentMenu = new MenuDto
                        {
                            Parent = null,
                            PermissionCode = menuAttribute.CodePermission,
                            Title = menuAttribute.Title,
                            Icon = menuAttribute.Icon,
                            Route = "#",
                            SortOrder = menuAttribute.SortOrder,
                        };
                        _menus.Add(parentMenu);
                        controller.GetMethods().Where(w => w.HasAttribute<MenuAttribute>()).ToList().ForEach(menu =>
                        {
                            var actionmenu = menu.GetCustomAttribute<MenuAttribute>();
                            _menus.Add(new MenuDto
                            {
                                Parent = parentMenu,
                                PermissionCode = actionmenu.CodePermission,
                                Title = actionmenu.Title,
                                Icon = actionmenu.Icon,
                                Route = $"/{(string.IsNullOrWhiteSpace(actionmenu.Action) ? actionmenu.CodePermission.Split('.').Last() : actionmenu.Action)}",
                                SortOrder = menuAttribute.SortOrder
                            });
                        });

                    }

                }
                result.Menus = _menus;
                result.Permission = _permissions;

            }
            return result;
        }


    }



    //    private string? GetRoute(Type controller, MethodInfo method)
    //    {

    //        var controllerRoute = controller.GetCustomAttribute<RouteAttribute>()?.Template;


    //        var methodRoute = method.GetCustomAttribute<RouteAttribute>()?.Template;


    //        return $"{controllerRoute}/{methodRoute}";
    //    }

    //    private string? GetHttpMethod(MethodInfo method)
    //    {

    //        if (method.GetCustomAttribute<HttpGetAttribute>() != null)
    //            return "GET";


    //        if (method.GetCustomAttribute<HttpPostAttribute>() != null)
    //            return "POST";


    //        if (method.GetCustomAttribute<HttpPutAttribute>() != null)
    //            return "PUT";


    //        if (method.GetCustomAttribute<HttpDeleteAttribute>() != null)
    //            return "DELETE";


    //        return null;
    //    }
    //}
}

