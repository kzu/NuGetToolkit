namespace ClariusLabs.NuGet.Toolkit
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using Microsoft.VisualStudio.Shell;

    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ShellExport
    {
        [Export]
        public IShellPackage Shell { get { return (IShellPackage)ServiceProvider.GlobalProvider.GetLoadedPackage(new Guid("9762932e-c8af-44f0-9071-e8257b5f2730")); } }
    }
}