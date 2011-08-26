using System;
using System.Collections.Generic;
using System.Linq;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Models
{
    public interface IPageSetup
    {
        void PreparePage(ICache cache, int itemsPerPage, Func<object,string> urlFactory);
    }

    public class Page<T> : IPageSetup
    {
        private readonly PagingArgs _paging;
        private readonly IQueryable<T> _query;

        public Page(PagingArgs paging, IQueryable<T> query)
        {
            _paging = paging;
            CurrentPage = _paging.Page;
            _query = query;
        }

        public int TotalCount { get; private set; }
        public int CurrentPage { get; private set; }
        public string UrlTemplate { get; private set; }
        public int ItemsPerPage { get; private set; }
        public string Title { get { return _paging.Title; } }
        public List<T> Elements { get; private set; }

        void IPageSetup.PreparePage(ICache cache, int itemsPerPage, Func<object, string> urlFactory)
        {
            SetupTotalCount(cache);
            ItemsPerPage = itemsPerPage;
            SetupUrlTemplate(urlFactory);
            PerformQuery(itemsPerPage);
        }

        private void SetupUrlTemplate(Func<object, string> urlFactory)
        {
            var url = urlFactory(_paging);
            var idx = url.LastIndexOf('/');
            UrlTemplate = url.Substring(0, idx + 1);
        }


        private void SetupTotalCount(ICache cache)
        {
            if (!cache.HasValue(TotalCountCacheKey))
                cache.Add(TotalCountCacheKey, _query.Count());
            TotalCount = cache.Get<int>(TotalCountCacheKey);
        }

        private void PerformQuery(int itemsPerPage)
        {
            Elements = _query.Skip(_paging.Page * itemsPerPage).Take(itemsPerPage).ToList();
        }

        private string TotalCountCacheKey
        {
            get { return _paging.TotalCountCacheKey; }
        }
    }
}