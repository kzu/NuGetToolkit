namespace ClariusLabs.NuGetToolkit
{
    using System;
    using System.Linq;

    internal static class Id
    {
        public const string Prefix = "ClariusLabs.NuGetToolkit";
        public const string PrefixDot = Prefix + ".";

        public static string For(string name)
        {
            return PrefixDot + name;
        }
    }
}