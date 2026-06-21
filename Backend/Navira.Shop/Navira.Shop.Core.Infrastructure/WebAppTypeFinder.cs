using System.Reflection;

namespace Navira.Shop.Core.Infrastructure
{
    public class WebAppTypeFinder : AppDomainTypeFinder
    {
        private bool _binFolderAssembliesLoaded;
        private readonly object _lock = new object(); // اضافه شدن آبجکت قفل

        public WebAppTypeFinder(ICoreFileProvider fileProvider = null) : base(fileProvider)
        {
        }

        public bool EnsureBinFolderAssembliesLoaded { get; set; } = true;

        public virtual string GetBinDirectory()
        {
            return AppContext.BaseDirectory;
        }

        public override IList<Assembly> GetAssemblies()
        {
            if (!EnsureBinFolderAssembliesLoaded || _binFolderAssembliesLoaded)
                return base.GetAssemblies();

            // استفاده از قفل برای جلوگیری از اجرای همزمان توسط چند Thread
            lock (_lock)
            {
                if (!_binFolderAssembliesLoaded)
                {
                    var binPath = GetBinDirectory();
                    LoadMatchingAssemblies(binPath);
                    _binFolderAssembliesLoaded = true;
                }
            }

            return base.GetAssemblies();
        }
    }

}
