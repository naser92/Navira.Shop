using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Identity
{
    public class PolicyPermissionModel : BaseReadModel<int>, IAuditableEntity
    {

        [ForeignKey("PermissionId")]
        public virtual PermissionModel Permission { get; set; }

        [ForeignKey("PolicyId")]
        public virtual PolicyModel Policy { get; set; }
        public int PermissionId { get; set; }
        public int PolicyId { get; set; }

    }
}
