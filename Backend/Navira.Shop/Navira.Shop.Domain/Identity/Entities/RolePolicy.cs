using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Identity
{
    public class RolePolicy : AggregateRoot<int>, IFullAuditableEntity<Guid>
    {
        [ForeignKey("PolicyId")]
        public virtual Policy Policy { get; set; }

        public string RoleId { get; set; }

        public int PolicyId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
