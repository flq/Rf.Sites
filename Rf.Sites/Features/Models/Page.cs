using System.Collections.Generic;
using System.Linq;

namespace Rf.Sites.Features.Models
{
    //public ContentTeaserPage

    public class Page<T>
    {
        private readonly PagingArgs _paging;
        private readonly IQueryable<T> _query;

        public Page(PagingArgs paging, IQueryable<T> query)
        {
            _paging = paging;
            CurrentPage = _paging.Page;
            _query = query;
        }

        public int TotalCount { get; set; }
        public int CurrentPage { get; private set; }

        public string Title { get { return _paging.Title; } }

        public IQueryable<T> Query
        {
            get { return _query; }
        }

        public string TotalCountCacheKey
        {
            get { return _paging.TotalCountCacheKey; }
        }

        public List<T> Elements { get; private set; }

        public void ExecuteQuery(int itemsPerPage)
        {
            Elements = _query.Skip(_paging.Page * itemsPerPage).Take(itemsPerPage).ToList();
        }
    }
}