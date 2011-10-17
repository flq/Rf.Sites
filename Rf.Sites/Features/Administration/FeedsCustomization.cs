﻿using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Administration
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
}