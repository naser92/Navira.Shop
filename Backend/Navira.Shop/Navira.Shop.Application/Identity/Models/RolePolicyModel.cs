using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Identity
{
    public class RolePolicyModel : BaseReadModel<int>, IAuditableEntity
    {

        public string RoleId { get; set; }

        public int PolicyId { get; set; }

        [ForeignKey("PolicyId")]
        public virtual PolicyModel Policy { get; set; }

    }
}
