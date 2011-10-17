using System;
using FubuCore.Reflection;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Administration
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
            {
                route.AddHttpMethodConstraint("GET");
                route.RemoveLastPatternPart();
                route.RemoveLastPatternPart();
                route.Append("{Id}");
            }

            if (route.Pattern.StartsWith("Tags", StringComparison.InvariantCultureIgnoreCase))
            {
                route.AddHttpMethodConstraint("GET");
                route.RemoveLastPatternPart();
                route.Append("tags");
            }
            if (route.Pattern.StartsWith("Post", StringComparison.InvariantCultureIgnoreCase))
            {
                route.AddHttpMethodConstraint("POST");
            }
            if (route.Pattern.StartsWith("Put", StringComparison.InvariantCultureIgnoreCase))
            {
                route.AddHttpMethodConstraint("Put");
                route.RemoveLastPatternPart();
                route.Append("{Id}");
            }

            route.Prepend("admin");
        }
    }
}