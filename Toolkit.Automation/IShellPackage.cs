namespace ClariusLabs.NuGetToolkit
{
    using System;
    using Clide;
    using Microsoft.VisualStudio.Shell;

    public interface IShellPackage : IServiceProvider
    {
        IDevEnv DevEnv { get; }
        ISelectedGraphNode SelectedNode { get; set; }
    }
}