namespace ClariusLabs.NuGet.Toolkit
{
    using System;
    using Clide;

    public interface IShellPackage : IServiceProvider
    {
        IDevEnv DevEnv { get; }
    }
}