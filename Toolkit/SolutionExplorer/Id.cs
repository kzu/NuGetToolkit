namespace ClariusLabs.NuGet.Toolkit
{
    using System;
    using System.Linq;

    internal static class Id
    {
        public const string Prefix = "ClariusLabs.NuGet.Toolkit";
        private const string prefixDot = Prefix + ".";

        public static string For(string name)
        {
            return prefixDot + name;
        }
    }
}