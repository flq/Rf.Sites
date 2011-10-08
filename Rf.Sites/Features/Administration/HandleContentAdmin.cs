using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuCore;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Features.Administration
{

    public class HandleContentAdmin : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            WireUp<GetBehavior>(graph, ActionCall.For<ContentAdmin>(ca => ca.Get(null)));
            WireUp<AdminActionBasicBehavior>(graph, ActionCall.For<ContentAdmin>(ca => ca.Tags()));
            WireUp<PutBehavior>(graph, ActionCall.For<ContentAdmin>(ca => ca.Put(new object())));
            WireUp<InsertBehavior>(graph, ActionCall.For<ContentAdmin>(ca => ca.Post(new object())));
        }

        private static void WireUp<T>(BehaviorGraph graph, ActionCall call) where T : IActionBehavior
        {
            var actionCall = graph.Actions().FirstOrDefault(ac => ac.Equals(call));
            if (actionCall != null)
             actionCall.AddBefore(new Wrapper(typeof(T)));
        }
    }

    
}