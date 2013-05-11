using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ExtensibilityHosting;

[assembly: AssemblyCompany("Clarius Labs")]
[assembly: AssemblyProduct("NuGet Toolkit")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Required to import/export MEF classes from this extension.
[assembly: VsCatalogName(NuPattern.ComponentModel.Composition.Catalog.DefaultCatalogName)]
[assembly: VsCatalogName(NuPattern.VisualStudio.Composition.Catalog.DefaultCatalogName)]