namespace ClariusLabs.NuGet.Toolkit
{
    using System;
    using Microsoft.VisualStudio.Shell;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell.Interop;
    using System.Diagnostics;
    using Clide;

#if DEBUG
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
#else
    [ProvideAutoLoad(UIContextGuids.SolutionExists)]
#endif
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource(1000, 5)]
    [Guid(Guids.PackageGuid)]
    [ProvideBindingPath]
    public class VsPackage : Package
    {
        protected override void Initialize()
        {
            base.Initialize();

#if DEBUG
            Tracer.Manager.SetTracingLevel("Toolkit", SourceLevels.All);
#else
			Tracer.Manager.SetTracingLevel("Toolkit", SourceLevels.Warning);
#endif

            Host.Initialize(this, "NuGet Tookit");
        }
    }
}