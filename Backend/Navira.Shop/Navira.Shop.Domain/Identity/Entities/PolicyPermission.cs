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

    }
}
