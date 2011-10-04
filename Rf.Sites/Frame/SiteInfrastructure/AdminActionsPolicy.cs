using System.Net;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class AdminActionsPolicy : BasicBehavior
    {
        private readonly RequestHeaders _headers;
        private readonly IOutputWriter _writer;

        public AdminActionsPolicy(RequestHeaders headers, IOutputWriter writer) : base(PartialBehavior.Executes)
        {
            _headers = headers;
            _writer = writer;
        }



        protected override DoNext performInvoke()
        {
            if (_headers["X-RfSite-Username"] == null || _headers["X-RfSite-Password"] == null)
            {
                _writer.WriteResponseCode(HttpStatusCode.Forbidden);
                return DoNext.Stop;
            }
            return base.performInvoke();
        }
    }
}