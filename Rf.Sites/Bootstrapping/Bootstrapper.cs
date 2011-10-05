using System;
using System.Reflection;
using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.Core.Runtime;
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

            Models.BindModelsWith<DynamicJsonBinding>();
            

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

    public class DynamicJsonBinding : IModelBinder
    {
        public bool Matches(Type type)
        {
            return type == typeof(Content);
        }

        public void Bind(Type type, object instance, IBindingContext context)
        {

        }

        public object Bind(Type type, IBindingContext context)
        {
            var data = context.Service<IStreamingData>();
            return new Content(data.InputText());
        }
    }
}