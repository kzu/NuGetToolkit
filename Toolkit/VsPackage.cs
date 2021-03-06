﻿namespace ClariusLabs.NuGetToolkit
{
    using System;
    using Microsoft.VisualStudio.Shell;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell.Interop;
    using System.Diagnostics;
    using Clide;
using System.ComponentModel.Composition;

#if DEBUG
    //[ProvideAutoLoad(UIContextGuids.NoSolution)]
#else
    [ProvideAutoLoad(UIContextGuids.SolutionExists)]
#endif
    [ProvideAutoLoad(UIContextGuids.SolutionExists)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource(1000, 5)]
    [Guid(Guids.PackageGuid)]
    [ProvideBindingPath]
    public class VsPackage : Package, IShellPackage
    {
        protected override void Initialize()
        {
            base.Initialize();

#if DEBUG
            Tracer.Manager.SetTracingLevel("Toolkit", SourceLevels.All);
#else
			Tracer.Manager.SetTracingLevel("Toolkit", SourceLevels.Warning);
#endif

            //ThreadHelper.Generic.Invoke(() =>
            //{
                Host.Initialize(this, "NuGet Toolkit");
                this.DevEnv = Clide.DevEnv.Get(this);
            //});
        }

        public IDevEnv DevEnv { get; private set; }
        public ISelectedGraphNode SelectedNode { get; set; }
    }
}