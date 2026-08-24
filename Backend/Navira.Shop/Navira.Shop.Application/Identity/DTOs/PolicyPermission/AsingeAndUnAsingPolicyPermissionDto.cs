namespace Navira.Shop.Application.Identity
{
    public class AsingeAndUnAsingPolicyPermissionDto
    {
        public int PolicyId { get; set; }
        public int[] PermissionAsinge { get; set; }
        public int[] PermissionUnAsinge { get; set; }
    }
}
