namespace ClariusLabs.NuGetToolkit
{
    using System;
    using Clide;

    public interface IPackageManagerConsole : IToolWindow
    {
        void Execute(string command);
    }
}