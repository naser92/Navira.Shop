using Navira.Shop.Application.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Identity
{
    public class PolicyPermissionModel : BaseReadModel<int>
    {

        [ForeignKey("PermissionId")]
        public virtual PermissionModel Permission { get; set; }

        [ForeignKey("PolicyId")]
        public virtual PolicyModel Policy { get; set; }
        public int PermissionId { get; set; }
        public int PolicyId { get; set; }

    }
}
