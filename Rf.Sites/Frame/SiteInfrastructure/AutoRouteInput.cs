using System.Diagnostics;
using System.Linq;
using FubuCore.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class AutoRouteInput
    {
        public bool Filter(ActionCall call)
        {
            return call.HandlerType.HasAttribute<HasActionsAttribute>() && call.HasInput;
        }

        public void Modification(IRouteDefinition route)
        {
            Debugger.Break();
            var props = from p in route.Input.InputType.GetProperties()
                        where p.CanWrite && p.CanRead && p.HasAttribute<RouteInputAttribute>()
                        select p;
            foreach (var p in props)
            {
                Debug.WriteLine(route.Input.InputType.Name + ", " + p.Name);
                var routeParameter = new RouteParameter(new SingleProperty(p));
                route.Input.AddRouteInput(routeParameter, true);
            }
        }
    }
}