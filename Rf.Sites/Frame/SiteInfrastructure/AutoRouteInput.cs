using System.Linq;
using FubuCore.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public static class AutoRouteInput
    {
        public static bool Filter(ActionCall call)
        {
            return call.HandlerType.HasAttribute<HasActionsAttribute>() && call.HasInput;
        }

        public static void Modification(IRouteDefinition route)
        {
            var props = from p in route.Input.InputType.GetProperties()
                        where p.CanWrite && p.CanRead && p.HasAttribute<RouteInputAttribute>()
                        select p;
            foreach (var p in props)
            {
                var routeParameter = new RouteParameter(new SingleProperty(p));
                route.Input.AddRouteInput(routeParameter, true);
            }
        }
    }
}