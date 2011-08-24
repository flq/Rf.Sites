using System.Linq;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Bootstrapping
{
    public class HandlePagingOutput : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            var activities = from a in graph.Actions()
                             let o = a.OutputType().BaseType
                             where o.IsGenericType && o.GetGenericTypeDefinition().Equals(typeof(Page<>))
                             let behavior = typeof(PagingBehavior<>).MakeGenericType(a.OutputType())
                             select new { Call = a, Behavior = behavior };

            foreach (var action in activities)
            {
                action.Call.AddAfter(new Wrapper(action.Behavior));
            }


        }
    }
}