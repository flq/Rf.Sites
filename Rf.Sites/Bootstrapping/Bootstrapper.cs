using System.Reflection;
using FubuMVC.Core;
using FubuMVC.Spark;
using FubuCore.Reflection;
using Rf.Sites.Features;
using Rf.Sites.Features.Administration;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Bootstrapping
{
    public class Bootstrapper : FubuRegistry
    {
        public Bootstrapper()
        {
            // This line turns on the basic diagnostics and request tracing
            IncludeDiagnostics(true);

            Actions
                .IncludeTypes(t => t.HasAttribute<HasActionsAttribute>());

            Policies
                .Add<HandleContentContinuation>()
                .Add<HandlePagingOutput>()
                .Add<HandleContentAdmin>();

            Output.ToJson.WhenTheOutputModelIs<IJsonResponse>();
            Output.To<StreamingOutput>().WhenTheOutputModelIs<IStreamOutput>();

            Models.BindModelsWith<DynamicBinding>();
            

            this.UseSpark();

            Routes
                .IgnoreControllerNamesEntirely()
                .IgnoreControllerNamespaceEntirely()
                .ModifyRouteDefinitions(InputParameterCustomization.Filter, InputParameterCustomization.Modification)
                .ModifyRouteDefinitions(ContentAdminCustomization.Filter, ContentAdminCustomization.Modification)
                .RootAtAssemblyNamespace()
                .HomeIs<PagedContent>(pc => pc.Home());

            Views.TryToAttach(ve => ve.by_ViewModel());
        }
    }
}