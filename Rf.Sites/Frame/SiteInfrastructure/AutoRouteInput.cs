using System;
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
            var inputType = route.Input.InputType;

            var props = from p in inputType.GetProperties()
                        let a = p.GetAttribute<RouteInputAttribute>()
                        where p.CanWrite && p.CanRead && a != null
                        select new { Property = p, Attribute = a };

            if (inputType.HasAttribute<RouteParameterSorterAttribute>())
            {
                var s = inputType.GetAttribute<RouteParameterSorterAttribute>();
                props = props.OrderBy(p => p.Property, s.GetComparer());
            }

            foreach (var p in props)
            {
                var routeParameter = new RouteParameter(new SingleProperty(p.Property));
                if (!string.IsNullOrEmpty(p.Attribute.DefaultValue))
                    routeParameter.DefaultValue = p.Attribute.DefaultValue;
                route.Input.AddRouteInput(routeParameter, true);
            }
        }
    }
}