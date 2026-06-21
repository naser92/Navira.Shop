using Microsoft.AspNetCore.Builder;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Infrastructure;


namespace Navira.Shop.Core.Web
{
    public interface IAppConfigService
    {
        int Order { get; }
        void Config(IApplicationBuilder app, ITypeFinder typeFinder, AppSettings appSetting);

    }
}
