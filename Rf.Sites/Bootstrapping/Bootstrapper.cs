using FubuMVC.Core;
using FubuCore.Reflection;
using Rf.Sites.Features;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Bootstrapping
{
    public class Bootstrapper : FubuRegistry
    {
        public Bootstrapper()
        {

            Actions.IncludeClassesSuffixedWithEndpoint();

            Policies.Add<HandlePagingOutput>();
            Policies.Add(p =>
            {
                p.Where.ResourceTypeImplements<IStreamOutput>();
                p.ModifyWith<StreamOutput>();
            });

            Routes
                .IgnoreControllerNamesEntirely()
                .IgnoreControllerNamespaceEntirely()
                .ModifyRouteDefinitions(InputParameterCustomization.Filter, InputParameterCustomization.Modification)
                .ModifyRouteDefinitions(FeedsCustomization.Filter, FeedsCustomization.Modification)
                .RootAtAssemblyNamespace()
                .HomeIs<HomeEndpoint>(pc => pc.Index());
        }
    }
}