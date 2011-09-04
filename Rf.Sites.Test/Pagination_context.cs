using System.Linq;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Test
{
    internal class Pagination_context : In_memory_repository_context<Content>
    {
        protected PagingBehavior<ContentTeaserPage> Pagebehavior;
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
            Pagebehavior = new PagingBehavior<ContentTeaserPage>(FubuRequest, _cache, null, _settings);
            SetItemsPerPage(3);
        }

        protected void SetItemsPerPage(int itemsPerPage)
        {
            _settings.ItemsPerPage = itemsPerPage;
        }

        protected void InvokeBehavior()
        {
            Page = new ContentTeaserPage(PagingArgs, _query);
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

        [TearDown]
        public void Reset()
        {
            Repository.Clear();
        }
    }
}