using Microsoft.VisualStudio.Shell;
namespace ClariusLabs.NuGet.Toolkit
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Dynamic;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using Clide.Solution;
    using Microsoft.Internal.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.GraphModel;
    using Microsoft.VisualStudio.GraphModel.Schemas;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using global::NuGet.VisualStudio;
    using EnvDTE;

    [GraphProvider(Name = Id.Prefix + ".GraphProvider")]
    public class ReferencesGraphProvider : IGraphProvider, IVsSelectionEvents
    {
        private static GraphCommandDefinition ShowInstalled = new GraphCommandDefinition(Id.For("ShowInstalled"), "Show Installed", GraphContextDirection.Target, 1);

        private static readonly ITracer tracer = Tracer.Get<ReferencesGraphProvider>();
        private GraphIcons icons;

        private uint selectionCookie;
        private IVsPackageInstallerServices manager;
        private DTE dte;

        private List<IGraphContext> trackingContext = new List<IGraphContext>();

        [ImportingConstructor]
        public ReferencesGraphProvider(
            [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
            IVsPackageInstallerServices manager)
        {
            this.manager = manager;
            this.dte = serviceProvider.GetService<DTE>();
            this.icons = new GraphIcons(serviceProvider);
            icons.Initialize();

            var monitorSelection = serviceProvider.GetService<IVsMonitorSelection>();
            if (monitorSelection != null)
            {
                monitorSelection.AdviseSelectionEvents(this, out selectionCookie);
            }
        }

        public void BeginGetGraphData(IGraphContext context)
        {
            this.icons.Initialize();

            this.BeginGetGraphData(context, true);
        }

        private void BeginGetGraphData(IGraphContext context, bool queueUserWorkItem)
        {
            if (context.Direction == GraphContextDirection.Self &&
                 context.RequestedProperties.Contains(DgmlNodeProperties.ContainsChildren))
            {
                var configNode = context.InputNodes.FirstOrDefault(node => node.IsPackagesConfigNode());
                if (configNode != null)
                {
                    using (var scope = new GraphTransactionScope())
                    {
                        this.TrackChanges(context);
                        configNode.SetValue(DgmlNodeProperties.ContainsChildren, true);

                        scope.Complete();
                    }
                }
            }
            else if (context.Direction == GraphContextDirection.Contains)
            {
                var configNode = context.InputNodes.FirstOrDefault(node => node.IsPackagesConfigNode());
                if (configNode != null)
                {
                    ThreadPool.QueueUserWorkItem(AddPackages, context);
                }
            }

            context.OnCompleted();
            return;

            if (context.Direction == GraphContextDirection.Custom)
            {
                StartSearch(context);
                return;
            }
            else if (context.Direction == GraphContextDirection.Target)
            {
                MessageBox.Show("Yay!");
            }

            context.OnCompleted();
        }

        private void AddPackages(object state)
        {
            IGraphContext context = (IGraphContext)state;
            
            // Selected item 


            // .GetProjectManager(null).LocalRepository.GetPackages()
            var progress = 0;
            var packages = manager.GetInstalledPackages().ToList();
            foreach (var package in manager.GetInstalledPackages())
            {
                using (var scope = new GraphTransactionScope())
                {
                    var node = context.Graph.Nodes.GetOrCreate(package.Id, package.Id, ReferencesGraphSchema.PackageCategory);
                    node.SetValue<string>(DgmlNodeProperties.Icon, GraphIcons.PackageIcon);

                    // Establish the relationship with the parent node.
                    foreach (GraphNode input in context.InputNodes)
                    {
                        context.Graph.Links.GetOrCreate(input, node, null, GraphCommonSchema.Contains);
                    }

                    // 50 is an arbitrary limit. 
                    context.ReportProgress(++progress, packages.Count, null);
                    context.OutputNodes.Add(node);
                    scope.Complete();
                }
                if (context.CancelToken.IsCancellationRequested)
                {
                    break;
                }

                context.CancelToken.ThrowIfCancellationRequested();
                System.Threading.Thread.Sleep(100);
            }

            context.OnCompleted();
        }

        private void TrackChanges(IGraphContext context)
        {
            if (context.TrackChanges && !this.trackingContext.Contains(context))
            {
                context.Canceled += context_Canceled;
                this.trackingContext.Add(context);
            }
        }

        void context_Canceled(object sender, EventArgs e)
        {
            var context = sender as IGraphContext;
            if (this.trackingContext.Contains(context))
            {
                this.trackingContext.Remove(context);
            }
        }

        delegate void SearchHandler(string term, IGraphContext context);
        ConcurrentQueue<IItemNode> searchItems;

        private void StartSearch(IGraphContext context)
        {
            var searchParameter = context.GetValue<ISolutionSearchParameters>(typeof(ISolutionSearchParameters).GUID.ToString());
            if (searchParameter != null)
            {
                string term = searchParameter.SearchQuery.SearchString;

                //var items = devEnv.SolutionExplorer().Solution
                //    .Traverse()
                //    .OfType<IItemNode>()
                //    .Where(item => DesignConstants.ConfigurationExtension.Equals(Path.GetExtension(item.PhysicalPath), StringComparison.OrdinalIgnoreCase));

                //searchItems = new ConcurrentQueue<IItemNode>();

                //foreach (var item in items)
                //{
                //    searchItems.Enqueue(item);
                //}

                Application.Current.Dispatcher.BeginInvoke(new SearchHandler(SearchNextItem), term, context);
            }
        }

        private void SearchNextItem(string term, IGraphContext context)
        {
            if (context.CancelToken.IsCancellationRequested || string.IsNullOrEmpty(term))
            {
                // stop right here, no more calls and empty the queue.
                this.searchItems = null;
                return;
            }

            IItemNode item;
            if (this.searchItems != null && this.searchItems.TryDequeue(out item))
            {
                //var model = this.mappingService.GetOrLoadModel(item.PhysicalPath);

                //var root = this.CreateRootNode(model);

                //foreach (var node in GetAllChildren(root))
                //{
                //    if (node.Name.ToLower().Contains(term.ToLower()))
                //    {
                //        // Ensure the file graph node is created
                //        var parentGraphNode = this.GetOrCreateFileGraphNode(context, item);

                //        // Ensure the root node is created
                //        parentGraphNode = this.GetOrCreateConfigurationGraphNode(context, parentGraphNode, root);

                //        // Create the parents
                //        foreach (var parentNode in this.GetParentHierarchy(node))
                //        {
                //            parentGraphNode = this.GetOrCreateConfigurationGraphNode(context, parentGraphNode, parentNode);
                //        }

                //        if (parentGraphNode.GetValue<INode>(ConfigurationGraphSchema.NodeProperty) != root)
                //        {
                //            // Create the node
                //            this.GetOrCreateConfigurationGraphNode(context, parentGraphNode, node);
                //        }
                //    }
                //}

                Application.Current.Dispatcher.BeginInvoke(new SearchHandler(SearchNextItem), term, context);
            }
            else
            {
                context.OnCompleted();
            }
        }

        private void AddPackagesNode(object state)
        {
            var context = (IGraphContext)state;
            using (var scope = new GraphTransactionScope())
            {
                var parentNode = context.InputNodes.First();
                var packagesNode = context.Graph.Nodes.GetOrCreate("packages-id", "Packages", ReferencesGraphSchema.PackagesCategory);
                packagesNode.Label = "Packages";

                packagesNode.SetValue(ReferencesGraphSchema.ConfigProperty, "packages.config");
                packagesNode.SetValue(DgmlNodeProperties.Icon, GraphIcons.PackagesIcon);

                // Sorting
                //graphNode.SetValue(CodeNodeProperties.SourceLocation, new SourceLocation(parent.GetId(), new Position(position, 0)));

                context.Graph.Links.GetOrCreate(parentNode, packagesNode, null, GraphCommonSchema.Contains);

                // TODO: need to monitor install/uninstall

                context.OutputNodes.Add(packagesNode);
                scope.Complete();
            }

            context.OnCompleted();
        }

        private void AddGraphNodes(object state)
        {
            var context = (IGraphContext)state;

            var parent = context.InputNodes.FirstOrDefault();
            //var parentNode = parent.GetValue<INode>(ConfigurationGraphSchema.NodeProperty);

            //var position = 0;
            //foreach (var node in parentNode.Children)
            //{
            //    var graphNode = this.GetOrCreateConfigurationGraphNode(context, parent, node, position++);
            //}

            //foreach (var graphNode in context.OutputNodes.ToList())
            //{
            //    if (!parentNode.Children.Contains(graphNode.GetValue<INode>(ConfigurationGraphSchema.NodeProperty)))
            //    {
            //        using (var scope = new GraphTransactionScope())
            //        {
            //            graphNode.Remove();

            //            scope.Complete();
            //        }
            //    }
            //}

            context.OnCompleted();
        }

        public IEnumerable<GraphCommand> GetCommands(IEnumerable<GraphNode> nodes)
        {
            return new GraphCommand[] 
            {          
                //new GraphCommand(ShowInstalled),
                new GraphCommand(GraphCommandDefinition.Contains, null, null, true)
            };
        }

        public T GetExtension<T>(GraphObject graphObject, T previous) where T : class
        {
            if (typeof(T) == typeof(IGraphFormattedLabel))
            {
                // TODO: format labels dynamically?
            }
            else if (typeof(T) == typeof(IGraphNavigateToItem))
            {
                return new GraphNodeNavigator() as T;
            }

            return null;
        }

        public Graph Schema { get { return null; } }

        public int OnCmdUIContextChanged(uint dwCmdUICookie, int fActive)
        {
            return VSConstants.S_OK;
        }

        public int OnElementValueChanged(uint elementid, object varValueOld, object varValueNew)
        {
            return VSConstants.S_OK;
        }

        SelectionService selectionService;

        public int OnSelectionChanged(IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld, IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew)
        {
            if (pSCNew != null)
            {
                try
                {
                    uint c;
                    pSCNew.CountObjects((uint)Microsoft.VisualStudio.Shell.Interop.Constants.GETOBJS_SELECTED, out c);

                    if (c == 1)
                    {
                        object[] objects = new object[c];
                        pSCNew.GetObjects((uint)Microsoft.VisualStudio.Shell.Interop.Constants.GETOBJS_SELECTED, c, objects);

                        var selectedGraphNode = objects.FirstOrDefault() as ISelectedGraphNode;
                        if (selectedGraphNode != null)
                        {
                            //var selectedNode = selectedGraphNode.Node.GetValue<object>(ReferencesGraphSchema.NodeProperty);

                            // TODO: grab the object we want to show in the properties grid.
                            var selectedNode = (object)new { File = "packages.config", HasDependencies = true };
                            if (selectedNode != null)
                            {
                                if (this.selectionService == null)
                                {
                                    // Get the service provider form Microsoft.VisualStudio.PlatformUI.HierarchyPivotNavigator
                                    IServiceProvider serviceProvider = pSCNew.AsDynamicReflection()._navigator.ServiceProvider;
                                    this.selectionService = new SelectionService(serviceProvider);
                                }

                                this.selectionService.Select(selectedNode);

                                System.Threading.Tasks.Task.Factory.StartNew(() =>
                                    {
                                        System.Threading.Thread.Sleep(10);

                                        object selectedElement = selectedNode;
                                        var browsablePattern = selectedNode as IBrowsablePattern;
                                        if (browsablePattern != null)
                                        {
                                            selectedElement = browsablePattern.GetBrowseObject();
                                        }

                                        ThreadHelper.Generic.Invoke(() => this.selectionService.Select(selectedElement));
                                    });
                            }

                        }
                    }
                }
                catch { }
            }

            return VSConstants.S_OK;
        }
    }
}