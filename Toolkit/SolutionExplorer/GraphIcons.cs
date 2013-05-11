using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Internal.VisualStudio.PlatformUI;

namespace ClariusLabs.NuGet.Toolkit
{
    public class GraphIcons
    {
        internal const string PackagesIcon = "ClariusLabs.NuGet.Toolkit.Packages";
        internal const string PackageIcon = "ClariusLabs.NuGet.Toolkit.Package";

        private IServiceProvider serviceProvider;

        public GraphIcons(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            Initialize();
        }

        public void Initialize()
        {
            RegisterIcon(PackagesIcon, "nuget.png");
            RegisterIcon(PackageIcon, "package.ico");
        }

        private void RegisterIcon(string imageName, string resourceName)
        {
            var imageService = this.serviceProvider.GetService(typeof(SVsImageService)) as IVsImageService;
            imageService.Add(imageName, new LazyImage(() => LoadWpfImage(resourceName)));
        }

        private ImageSource LoadWpfImage(string resourceName)
        {
            string prefix = "pack://application:,,,/" + this.GetType().Assembly.GetName().Name + ";component/Resources/";
            string fullResourceName = prefix + resourceName;

            return new BitmapImage(new Uri(fullResourceName));
        }
    }
}
