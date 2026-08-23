using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Identity
{
    public class RolePolicy : FullEntity<int>, IFullAuditableEntity<Guid>
    {
        [ForeignKey("PolicyId")]
        public virtual Policy Policy { get; set; }

        public Guid RoleId { get; set; }

        public int PolicyId { get; set; }

        public RolePolicy() { }

        public RolePolicy(Guid roleId, int policyId)
        {
            RoleId = roleId;
            PolicyId = policyId;
        }

        public static RolePolicy AssingePolicy(Guid roleId, int policyId) =>
                            new RolePolicy(roleId, policyId);


        public static IList<RolePolicy> AssingePolicy(Guid roleId, int[] policyIds)
        {
            var rolePolicy = new List<RolePolicy>();

            foreach (var policyId in policyIds)
            {
                rolePolicy.Add(AssingePolicy(roleId, policyId));
            }

            return rolePolicy;
        }


    }
}
