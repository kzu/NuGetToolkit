namespace ClariusLabs.NuGet.Toolkit
{
    using Microsoft.VisualStudio.GraphModel;
    using Microsoft.VisualStudio.GraphModel.Schemas;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using global::NuGet;

    internal static class GraphNodeExtensions
    {
        internal static bool IsRootNode(this GraphNode node)
        {
            return node.IsPropertiesNode() || node.IsPackagesConfigNode();
        }

        internal static bool IsPropertiesNode(this GraphNode node)
        {
            return node.HasCategory("ProjectFolder") && node.Label == "Properties";
        }

        internal static bool IsPackagesConfigNode(this GraphNode node)
        {
            return node.HasCategory(CodeNodeCategories.ProjectItem) && node.Label == "packages.config";
        }

        internal static bool IsPackagesNode(this GraphNode node)
        {
            return node.HasCategory(ReferencesGraphSchema.PackagesCategory);
        }

        internal static bool IsPackageNode(this GraphNode node)
        {
            return node.HasCategory(ReferencesGraphSchema.PackageCategory);
        }
    }
}
