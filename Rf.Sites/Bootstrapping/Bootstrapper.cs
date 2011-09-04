using FubuMVC.Core;
using FubuMVC.Spark;
using FubuCore.Reflection;
using Rf.Sites.Features;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Bootstrapping
{
    public class Bootstrapper : FubuRegistry
    {
        public Bootstrapper()
        {
            // This line turns on the basic diagnostics and request tracing
            IncludeDiagnostics(true);

            Actions.IncludeTypes(t => t.HasAttribute<HasActionsAttribute>());

            Policies
                .Add<HandleContentContinuation>()
                .Add<HandlePagingOutput>();

            Output.ToJson.WhenTheOutputModelIs<JsonResponse>();

            this.UseSpark();

            Routes
                .IgnoreControllerNamesEntirely()
                .IgnoreControllerNamespaceEntirely()
                .ModifyRouteDefinitions(AutoRouteInput.Filter, AutoRouteInput.Modification)
                .RootAtAssemblyNamespace()
                .HomeIs<PagedContent>(pc => pc.Home());

            Views.TryToAttach(ve => ve.by_ViewModel());
        }
    }
}