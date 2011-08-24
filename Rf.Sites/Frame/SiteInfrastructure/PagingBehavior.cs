using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class PagingBehavior<T> : BasicBehavior where T : class, IPageSetup
    {
        private readonly IFubuRequest _request;
        private readonly ICache _cache;
        private readonly PaginationSettings _settings;

        public PagingBehavior(IFubuRequest request, ICache cache, PaginationSettings settings) : base(PartialBehavior.Executes)
        {
            _request = request;
            _cache = cache;
            _settings = settings;
        }

        protected override FubuMVC.Core.DoNext performInvoke()
        {
            var page = _request.Get<T>();
            if (page != null)
                page.PreparePage(_cache, _settings.ItemsPerPage);
            return base.performInvoke();
        }
    }
}