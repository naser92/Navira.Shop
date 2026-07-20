using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navira.Shop.Core.ViewModels
{
    public sealed class MenuDto
    {
        public string Title { get; init; } = default!;

        public string? Parent { get; init; }

        public int Order { get; init; }

        public string? Icon { get; init; }
    }
}
