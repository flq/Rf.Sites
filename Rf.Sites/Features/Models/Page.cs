using System;
using System.Collections.Generic;
using System.Linq;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Models
{
    public interface IPageSetup
    {
        void PreparePage(ICache cache, int itemsPerPage);
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
        public int Pages { get; private set; }
        public string Title { get { return _paging.Title; } }
        public List<T> Elements { get; private set; }

        void IPageSetup.PreparePage(ICache cache, int itemsPerPage)
        {
            SetupTotalCount(cache);
            SetupTotalPageCount(itemsPerPage);
            PerformQuery(itemsPerPage);
        }

        private void SetupTotalCount(ICache cache)
        {
            if (!cache.HasValue(TotalCountCacheKey))
                cache.Add(TotalCountCacheKey, _query.Count());
            TotalCount = cache.Get<int>(TotalCountCacheKey);
        }

        private void SetupTotalPageCount(int itemsPerPage)
        {
            Pages = (int)Math.Ceiling((decimal)TotalCount / itemsPerPage);
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