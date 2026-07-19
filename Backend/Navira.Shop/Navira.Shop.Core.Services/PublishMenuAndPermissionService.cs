using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Infrastructure;
using Navira.Shop.Core.ViewModels;
using NaviraShop.Core.Mq;
using SmartFormat;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Navira.Shop.Core.Services
{
    public class PublishMenuAndPermissionService : IPublishMenuAndPermissionService
    {
        private PermissionDto _permission;
        //private List<MenuDto> _menus;
        private readonly IPublisher _publisher;
        private readonly ITypeFinder _typeFinder;
        private readonly Guid _actionBasePermissionType = Guid.Parse("08A81E4A-AF2A-4840-9216-76C231105E9E");
        private readonly Guid _menuBasePermissionType = Guid.Parse("6528E860-4874-44D0-B4D3-858F88E847CF");
        private Guid _subsystemId;
        private string _subSystemName;
        private readonly AppSettings _appSettings;

        public PublishMenuAndPermissionService(IPublisher publisher, ITypeFinder typeFinder, AppSettings appSettings)
        {
            _publisher = publisher;
            _typeFinder = typeFinder;
            _appSettings = appSettings;
        }
        public void BuildPermissionAndMenu(params string[] controllerNames)
        {


            var controllers = _typeFinder.FindClassesOfType<Controller>();

            if (controllerNames.IsNotNullOrEmpty())
                controllers = controllers.Where(x => controllerNames.Any(y => y == x.Name));

            //_menus = new List<MenuDto>();
            _permission = AddPermission(null, "", _subSystemName.ToLower(), _appSettings.SystemInfo.Title, _actionBasePermissionType, null);

            if (controllers.IsNotNullOrEmpty())
            {
                foreach (var controller in controllers)
                {
                    var display = controller.GetCustomAttribute<DisplayAttribute>();
                    var controllerTitle = display.IsNull() ? "" : display.Name;
                    var controllerName = controller.Name.Replace("Controller", "");
                    //- دسترسی کنترلر

                    var permissionAttribute = controller.GetCustomAttribute<PermissionAttribute>();
                    var apiName = "";

                    var menuAttribute = controller.GetCustomAttribute<MenuAttribute>();
                    MenuDto parentMenu;

                    if (!permissionAttribute.IsNull())
                    {

                        apiName = Smart.Format(permissionAttribute.Name, new { ControllerName = controllerName }).ToLower();
                        var apiTitle = Smart.Format(permissionAttribute.Title, new { ControllerTitle = controllerTitle });
                        var menuName = string.IsNullOrEmpty(permissionAttribute.MenuName) ? null : Smart.Format(permissionAttribute.MenuName, new { ControllerTitle = controllerTitle }).ToLower();

                        var controllerPermission = AddPermission(_permission,
                            controllerName,
                            apiName,
                            apiTitle,
                            _actionBasePermissionType,
                            menuAttribute.IsNull() ? null : menuName);

                        //دسترسی به اکشن ها
                        controller.GetMethods().Where(w => w.HasAttribute<PermissionAttribute>()).ToList().ForEach(action =>
                        {
                            var actionDisplay = action.GetCustomAttribute<DisplayAttribute>();
                            var actionTitle = actionDisplay.IsNull() ? "" : actionDisplay.Name;

                            var actionName = action.Name;

                            var actionPermission = action.GetCustomAttribute<PermissionAttribute>();

                            var permissionName = Smart.Format(actionPermission.Name, new { ActionName = actionName, ControllerName = controllerName }).ToLower();
                            var permissionTitle = Smart.Format(actionPermission.Title, new { ActionTitle = actionTitle, ControllerTitle = controllerTitle });
                            var menuName = string.IsNullOrEmpty(actionPermission.MenuName) ? null : Smart.Format(actionPermission.MenuName, new { ActionName = actionName, ControllerName = controllerName }).ToLower();

                            AddPermission(controllerPermission,
                                controllerName,
                                permissionName,
                                permissionTitle,
                                _actionBasePermissionType,
                                menuAttribute.IsNull() ? null : menuName);
                        });
                    }

                    if (!menuAttribute.IsNull())
                    {
                        parentMenu = AddMenu(null, Smart.Format(menuAttribute.Title, new { ControllerTitle = controllerTitle }), Smart.Format(menuAttribute.Name, new
                        {
                            ControllerName = controllerName
                        }).ToLower(), menuAttribute.Icon, $"{(menuAttribute.Action.IsNullOrWhiteSpace() ? "#" : $"/{_subSystemName}/{menuAttribute.Action}")}", menuAttribute.Priority, controllerName, menuAttribute.AdminInaccessible);

                        controller.GetMethods().Where(w => w.HasAttribute<MenuAttribute>()).ToList().ForEach(menu =>
                        {
                            var actionDisplay = menu.GetCustomAttribute<DisplayAttribute>();
                            var actionTitle = actionDisplay.IsNull() ? "" : actionDisplay.Name;
                            var actionName = menu.Name;

                            var actionmenu = menu.GetCustomAttribute<MenuAttribute>();

                            var name = Smart.Format(actionmenu.Name, new { ActionName = actionName, ControllerName = controllerName }).ToLower();
                            AddMenu(parentMenu, Smart.Format(actionmenu.Title, new { ActionTitle = actionTitle, ControllerTitle = controllerTitle }), name, actionmenu.Icon, $"/{_subSystemName}{(name.IsNullOrWhiteSpace() ? "" : "/" + name)}{(actionmenu.Action.IsNullOrWhiteSpace() ? "" : "/" + actionmenu.Action)}", actionmenu.Priority, controllerName, actionmenu.AdminInaccessible);
                        });
                    }
                }


            }

            _publisher.Publish(new RegisterMenuAndPermissionEvent(_permission, _menus));
        }

        private PermissionDto AddPermission(PermissionDto parent, string controllerName, string name, string title, Guid typeid, string menuName)
        {
            var itemInDb = parent?.Childs.SingleOrDefault(s => s.Name == name && s.BasePermissionTypeId == typeid);
            if (itemInDb == null)
            {
                itemInDb = new PermissionDto
                {
                    ControllerName = controllerName,
                    Name = name,
                    BasePermissionTypeId = typeid,
                    SubSystemId = _subsystemId,
                    Title = title.IsNullOrWhiteSpace() ? name : title,
                    Childs = new List<PermissionDto>(),
                    MenuName = menuName
                };
                parent?.Childs.Add(itemInDb);
            }



            return itemInDb;
        }

        private MenuDto AddMenu(MenuDto parent, string title, string titleLatin, string icon, string url, int priority, string controllerName, bool isAdminInaccessible)
        {
            var itemInDb = parent == null
                    ? _menus.SingleOrDefault(x => x.TitleLatin == titleLatin && x.Url == url.ToLower())
                    : parent.Childs.SingleOrDefault(s => s.TitleLatin == titleLatin && s.Url == url.ToLower());

            if (itemInDb == null)
            {
                itemInDb = new MenuDto()
                {
                    SubSystemId = _subsystemId,
                    Url = url.ToLower(),
                    BasePermissionTypeId = _menuBasePermissionType,
                    FontIcon = icon,
                    Priority = priority,
                    Title = title.IsNullOrWhiteSpace() ? titleLatin : title,
                    TitleLatin = titleLatin,
                    ControllerName = controllerName,
                    Childs = new List<MenuDto>(),
                    AdminInaccessible = isAdminInaccessible
                };
                if (parent == null)
                    _menus.Add(itemInDb);
                else
                    parent.Childs.Add(itemInDb);
            }
            return itemInDb;
        }
    }
}

