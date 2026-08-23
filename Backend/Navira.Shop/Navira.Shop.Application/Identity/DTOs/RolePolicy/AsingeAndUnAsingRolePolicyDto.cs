namespace Navira.Shop.Application.Identity
{
    public class AsingeAndUnAsingRolePolicyDto
    {
        public Guid RoleId { get; set; }
        public int[] PolicyAsinge { get; set; }
        public int[] PolicyUnAsinge { get; set; }
    }
}
