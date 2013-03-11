using FubuMVC.Core;
using FubuCore.Reflection;
using Rf.Sites.Features;
using Rf.Sites.Features.Models;
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

            Policies.Add(p =>
            {
                p.Where.InputTypeIs<ContentId>();
                p.Add.BehaviorToEnd<SingleContentEndpoint.NullTo404Behavior>();
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