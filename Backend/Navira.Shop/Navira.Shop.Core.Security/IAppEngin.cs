namespace Navira.Shop.Core.Security
{
    public interface IAppEngin
    {
        Guid UserId { get; }
        Guid? TraceId { get; }
        bool IsAdmin { get; }
        int? UserType { get; }
        UserInfoDto GetCurrentUserInfo();
    }
}
