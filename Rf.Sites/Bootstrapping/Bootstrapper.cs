using System;
using FubuMVC.Core;
using Rf.Sites.Features;
using FubuCore.Reflection;
using Rf.Sites.Frame;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Bootstrapping
{
    public class Bootstrapper : FubuRegistry
    {
        public Bootstrapper()
        {
            // This line turns on the basic diagnostics and request tracing
            IncludeDiagnostics(true);

            Actions.IncludeTypes(t => t.HasAttribute<HasActionsAttribute>());

            Policies.Add<HandleContentContinuation>();

            Routes
                .IgnoreControllerNamespaceEntirely()
                .ModifyRouteDefinitions(AutoRouteInput.Filter, AutoRouteInput.Modification)
                .RootAtAssemblyNamespace();

            // Match views to action methods by matching
            // on model type, view name, and namespace
            Views.TryToAttachWithDefaultConventions();
        }
    }
}