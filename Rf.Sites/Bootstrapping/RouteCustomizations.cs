using FubuCore.Reflection;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;
using Rf.Sites.Features;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Bootstrapping
{
    public static class FeedsCustomization
    {
        public static bool Filter(ActionCall call)
        {
            return call.HasInput && call.InputType() == typeof(InputFeedTag);
        }

        public static void Modification(IRouteDefinition route)
        {
            route.Replace("Feed", "");
            route.AddHttpMethodConstraint("GET");
        }
    }

    public static class InputParameterCustomization
    {
        public static bool Filter(ActionCall call)
        {
            return call.HandlerType.Name.EndsWith("Endpoint") && call.HasInput && call.InputType().HasAttribute<RouteParameterSorterAttribute>();
        }

        public static void Modification(IRouteDefinition route)
        {
            var inputType = route.Input.InputType;
            var s = inputType.GetAttribute<RouteParameterSorterAttribute>();
            route.ReorderParts(s.GetComparer());
        }
    }
}