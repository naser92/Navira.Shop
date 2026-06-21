namespace Navira.Shop.Core.Security
{
    public class UserInfoDto
    {
        public Guid? UserId { get; set; }
        public Guid? TraceId { get; set; }
        public int? UserType { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
