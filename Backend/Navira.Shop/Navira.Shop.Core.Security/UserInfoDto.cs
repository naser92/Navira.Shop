namespace Navira.Shop.Core.Security
{
    public class UserInfoDto
    {
        public Guid? UserId { get; set; }
        public string? SubjectId { get; set; }
        public string? UserName { get; set; }
        public string? PreferredUsername { get; set; }
        public string? Email { get; set; }
        public Guid? TraceId { get; set; }
        public int? UserType { get; set; }
        public bool IsAdmin { get; set; } = false;
        public List<string> Roles { get; set; } = new();
    }
}
