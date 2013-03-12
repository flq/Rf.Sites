using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class NullTo404Behavior<TOut> : BasicBehavior where TOut : class
    {
        private readonly IFubuRequest _request;
        private readonly IHttpWriter _writer;
        private readonly IUrlRegistry _urls;

        public NullTo404Behavior(IFubuRequest request, IHttpWriter writer, IUrlRegistry urls)
            : base(PartialBehavior.Executes)
        {
            _request = request;
            _writer = writer;
            _urls = urls;
        }

        protected override DoNext performInvoke()
        {
            var output = _request.Get<TOut>();
                
            if (output == null)
            {
                var url = _urls.UrlFor(new RedirectTo404());
                _writer.Redirect(url);
                return DoNext.Stop;
            }
                
            return DoNext.Continue;
        }
    }
}