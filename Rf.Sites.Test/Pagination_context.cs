using System.Linq;
using FubuMVC.Core.Runtime;
using Rf.Sites.Entities;
using Rf.Sites.Features;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Test
{
    internal class Pagination_context : In_memory_repository_context<Content>
    {
        protected PagingBehavior<ContentTeaserVM> Pagebehavior;
        protected InMemoryFubuRequest FubuRequest;
        protected PagingArgs PagingArgs;
        protected Page<ContentTeaserVM> Page;

        private readonly PaginationSettings _settings = new PaginationSettings();
        private readonly ICache _cache = new InMemoryCache();
        private IQueryable<ContentTeaserVM> _query;

        public Pagination_context()
        {
            FubuRequest = new InMemoryFubuRequest();
            _cache = new InMemoryCache();
            Pagebehavior = new PagingBehavior<ContentTeaserVM>(FubuRequest, _cache, _settings);
            SetItemsPerPage(3);
        }

        protected void SetItemsPerPage(int itemsPerPage)
        {
            _settings.ItemsPerPage = itemsPerPage;
        }

        protected void InvokeBehavior()
        {
            Page = new Page<ContentTeaserVM>(PagingArgs, _query);
            FubuRequest.SetObject(Page);
            Pagebehavior.Invoke();
        }

        protected void SetPagingArgs(PagingArgs pagingArgs)
        {
            PagingArgs = pagingArgs;
        }

        protected void SetQuery(IQueryable<ContentTeaserVM> query)
        {
            _query = query;
        }
    }
}