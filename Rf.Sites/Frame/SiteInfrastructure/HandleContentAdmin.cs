using System.Linq;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using Rf.Sites.Features.Administration;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class HandleContentAdmin : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            var activities = from a in graph.Actions()
                             where a.HandlerType.Equals(typeof(ContentAdmin))
                             select a;

            foreach (var action in activities)
            {
                action.AddBefore(new Wrapper(typeof(AdminActionsPolicy)));
            }
        }
    }
}