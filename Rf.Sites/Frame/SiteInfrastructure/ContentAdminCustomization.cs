using System;
using FubuCore.Reflection;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;
using Rf.Sites.Features.Administration;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public static class ContentAdminCustomization
    {
        public static bool Filter(ActionCall call)
        {
            return call.HandlerType.HasAttribute<HasActionsAttribute>() && call.HandlerType.Equals(typeof(ContentAdmin));
        }

        public static void Modification(IRouteDefinition route)
        {
            if (route.Pattern.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase))
              route.AddHttpMethodConstraint("GET");
            if (route.Pattern.StartsWith("Post", StringComparison.InvariantCultureIgnoreCase))
              route.AddHttpMethodConstraint("POST");
            if (route.Pattern.StartsWith("Put", StringComparison.InvariantCultureIgnoreCase))
              route.AddHttpMethodConstraint("Put");

            route.Prepend("admin");
        }
    }
}