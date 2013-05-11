namespace ClariusLabs.NuGet.Toolkit
{
    using Microsoft.VisualStudio.GraphModel;

    public class ReferencesGraphSchema
    {
        public static readonly GraphSchema Schema = new GraphSchema("NuGetReferencesSchema");
        public static readonly GraphCategory PackagesCategory = Schema.Categories.AddNewCategory("NuGetReferencesSchema_Packages");
        public static readonly GraphCategory PackageCategory = Schema.Categories.AddNewCategory("NuGetReferencesSchema_Package");
        public static readonly GraphProperty IdProperty = Schema.Properties.AddNewProperty("NuGetReferencesSchema_Package_Id", typeof(string));

        public static readonly GraphProperty ConfigProperty = Schema.Properties.AddNewProperty("NuGetReferencesSchema_Package_Config", typeof(string));
    }
}