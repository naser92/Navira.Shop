using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Core.ViewModels;
using Navira.Shop.Domain.Identity;

namespace Navira.Shop.Application.Identity
{
    public class RegisterPermissionAndMenuCommandHandler : CommandHandler, ICommandHandler<RegisterPermissionAndMenuCommand>
    {
        private readonly IPermissionQueryService _permissionQueryService;
        private readonly IPermissionWriteRepository _permissionWriteRepository;
        private readonly IMenuWriteRepository _menuWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterPermissionAndMenuCommandHandler(IUnitOfWork uow, IPermissionQueryService permissionQueryService, IPermissionWriteRepository permissionWriteRepository, IMenuWriteRepository menuWriteRepository, IUnitOfWork unitOfWork) : base(uow)
        {
            _permissionQueryService = permissionQueryService;
            _permissionWriteRepository = permissionWriteRepository;
            _menuWriteRepository = menuWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult> Handle(RegisterPermissionAndMenuCommand command, CancellationToken cancellationToken = default)
        {
            //var permissionExist = await _permissionQueryService.GetByCode<PermissionDto>(command.data.Permission.Select(x => x.Code).ToList());
            var permissionExist = await _permissionWriteRepository.GetAll(x => x.Where(i => (command.data.Permission.Select(x => x.Code)).Contains(i.Code)));

            foreach (var item in command.data.Permission)
            {
                if ((permissionExist.Select(x => x.Code)).Contains(item.Code))
                {
                    var permission = permissionExist.First(x => x.Code == item.Code);

                    permission.ChangeTitle(item.Title);
                }
                else
                {
                    var permission = Permission.Create(
                        item.Scope,
                        item.ControllerName,
                        item.Title,
                        item.Code
                        );

                    await _permissionWriteRepository.Insert(permission);
                    var a = 1;
                }
            }
            await _unitOfWork.Commit();
            var permissionMenuCode = await _permissionQueryService.GetByCode<PermissionDto>(command.data.Menus.Select(x => x.PermissionCode).ToList());

            foreach (var item in command.data.Menus)
            {
                if (permissionMenuCode.Any(x => x.Code == item.PermissionCode))
                {
                    var permissionId = permissionMenuCode.FirstOrDefault(x => x.Code == item.PermissionCode).Id;
                    int? parentId = item.Parent is null ? null : permissionMenuCode.FirstOrDefault(x => x.Code == item.Parent.PermissionCode).Id;
                    var menu = await _menuWriteRepository.Get(x => x.PermissionId == permissionId);

                    if (menu != null)
                    {
                        menu.SetTitle(item.Title);
                        menu.SetRoute(item.Route);
                        menu.SetIcon(item.Icon);
                    }
                    else
                    {
                        menu = Menu.Create(parentId, permissionId, item.SortOrder);
                        menu.SetTitle(item.Title);
                        menu.SetRoute(item.Route);
                        menu.SetIcon(item.Icon);
                        await _menuWriteRepository.Insert(menu);
                    }
                }
                else
                {
                    throw new Exception("Menu Permission not Exists .");
                }
            }

            return Result.Success("پردازش انجام شد");
        }
    }
}
