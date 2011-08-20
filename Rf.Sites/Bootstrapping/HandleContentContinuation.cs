using System.Linq;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using Rf.Sites.Features;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Bootstrapping
{
    public class HandleContentContinuation : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            var activities = from a in graph.Actions()
                             let o = a.OutputType()
                             where o.IsGenericType && o.GetGenericTypeDefinition().Equals(typeof(ContentContinuation<,>))
                             let types = new { ContentType = o.GetGenericArguments()[0], WrapperType = o.GetGenericArguments()[1] }
                             let behavior = typeof(ContentContinuationBehavior<,>).MakeGenericType(types.ContentType, types.WrapperType)
                             select new { Call = a, Behavior = behavior };
            foreach (var action in activities)
            {
                action.Call.AddAfter(new Wrapper(action.Behavior));
            }


        }
    }
}