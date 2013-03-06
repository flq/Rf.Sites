using System;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class PagingBehavior<T> : BasicBehavior where T : class, IPageSetup
    {
        private readonly IFubuRequest _request;
        private readonly ICache _cache;
        private readonly PaginationSettings _settings;
        private Func<object,string> _registry;

        public PagingBehavior(
            IFubuRequest request, 
            ICache cache, 
            IUrlRegistry registry,
            PaginationSettings settings) : base(PartialBehavior.Executes)
        {
            _request = request;
            _cache = cache;
            _registry = registry != null ? new Func<object, string>(o => registry.UrlFor(o)) : model => "No registry";
            _settings = settings;
        }

        protected override FubuMVC.Core.DoNext performInvoke()
        {
            var page = _request.Get<T>();
            if (page != null)
                page.PreparePage(_cache, _settings.ItemsPerPage, _registry);
            return base.performInvoke();
        }
    }
}