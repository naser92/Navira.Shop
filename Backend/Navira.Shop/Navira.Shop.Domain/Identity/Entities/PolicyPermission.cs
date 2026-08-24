using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Identity
{
    public class PolicyPermission : FullEntity<int>, IFullAuditableEntity<Guid>
    {
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }

        [ForeignKey("PolicyId")]
        public virtual Policy Policy { get; set; }
        public int PolicyId { get; set; }
        public int PermissionId { get; set; }

        public PolicyPermission() { }

        public PolicyPermission(int policyId, int permissionId)
        {
            PolicyId = policyId;
            PermissionId = permissionId;
        }

        public static PolicyPermission Create(int policyId, int permissionId) =>
             new PolicyPermission(policyId, permissionId);



        public static List<PolicyPermission> AssingePermission(int policyId, int[] permissionIds)
        {
            var result = new List<PolicyPermission>();
            foreach (var permissionId in permissionIds)
            {
                result.Add(Create(policyId, permissionId));
            }

            return result;
        }




    }
}
