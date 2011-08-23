using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Rf.Sites.Features;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class PagingBehavior<T> : BasicBehavior
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

        protected override void afterInsideBehavior()
        {
            var page = _request.Get<Page<T>>();
            if (page == null)
                return;

            if (!_cache.HasValue(page.TotalCountCacheKey))
                _cache.Add(page.TotalCountCacheKey, page.Query.Count());

            page.TotalCount = _cache.Get<int>(page.TotalCountCacheKey);
            page.ExecuteQuery(_settings.ItemsPerPage);

        }
    }
}