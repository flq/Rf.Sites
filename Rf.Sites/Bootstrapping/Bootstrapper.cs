using System;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;

namespace Rf.Sites.Bootstrapping
{
    public class Bootstrapper : FubuRegistry
    {
        public Bootstrapper()
        {
            // This line turns on the basic diagnostics and request tracing
            IncludeDiagnostics(true);

            // All public methods from concrete classes ending in "Controller"
            // in this assembly are assumed to be action methods
            Actions.IncludeClassesSuffixedWithController();

            Policies.AlterActions(call =>
            {
                var checkerType = typeof(NullModelIs404<>).MakeGenericType(call.OutputType());
                call.AddAfter(new Wrapper(checkerType));
            });

            Routes
                .IgnoreControllerNamespaceEntirely()
                .RootAtAssemblyNamespace();

            // Match views to action methods by matching
            // on model type, view name, and namespace
            Views.TryToAttachWithDefaultConventions();
        }
    }

    public class NullModelIs404<T> : BasicBehavior where T : class
    {
        private readonly IFubuRequest _request;
        private readonly IContinuationDirector _continuation;

        public NullModelIs404(IFubuRequest request, IContinuationDirector continuation) : base(PartialBehavior.Executes)
        {
            _request = request;
            _continuation = continuation;
        }

        protected override void afterInsideBehavior()
        {
            var output = _request.Get<T>();
            if (output == null)
                _continuation.RedirectTo(new ResourceNotFound());
        }
    }

    public class ResourceNotFound
    {
    }
}