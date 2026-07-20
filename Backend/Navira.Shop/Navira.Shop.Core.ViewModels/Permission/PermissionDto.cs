using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navira.Shop.Core.ViewModels
{
    public class PermissionDto
    {
        public int BaseSubSystemId { get; set; }

        public string Scope { get; set; }

        public string Title { get; set; }

        public string ControllerName { get; set; }

        
    }
}
