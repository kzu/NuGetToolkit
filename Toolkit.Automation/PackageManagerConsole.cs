namespace ClariusLabs.NuGetToolkit
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using Clide;
    using Clide.VisualStudio;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    [Export(typeof(IToolWindow))]
    [Export(typeof(IPackageManagerConsole))]
    internal class PackageManagerConsole : IPackageManagerConsole
    {
        private VsToolWindow toolWindow;
        private IServiceProvider serviceProvider;

        [ImportingConstructor]
        public PackageManagerConsole([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.toolWindow = new VsToolWindow(serviceProvider, new Guid("0AD07096-BBA9-4900-A651-0598D26F6D24"));
        }

        public void Close()
        {
            toolWindow.Close();
        }

        public bool IsVisible
        {
            get { return toolWindow.IsVisible; }
        }

        public void Show()
        {
            toolWindow.Show();
        }

        public void Execute(string command)
        {
            object windowPane;
            ErrorHandler.ThrowOnFailure(toolWindow.Frame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out windowPane));
            // \o/: there should be a very easy way to bring up the console and execute a command :)
            ((dynamic)windowPane).Execute(command, null);
        }
    }
}