using System;
using System.Linq;
using FubuMVC.Core;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features
{
    [HasActions]
    public class PagedContent
    {
        private readonly IRepository<Content> _contentRepository;

        public PagedContent(IRepository<Content> contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public ContentTeaserPage Home()
        {
            return GetContentDefault(new PagingArgs { Page = 0 });
        }

        [UrlPattern("all/{Page}")]
        public ContentTeaserPage GetContentDefault(PagingArgs paging)
        {
            var query = from c in _contentRepository
                        where c.Created < DateTime.Now.ToUniversalTime()
                        orderby c.Created descending
                        select new ContentTeaserVM(c.Id, c.Title, c.Created, c.Teaser);
            var page = new ContentTeaserPage(paging, query);
            return page;
        }
    }
}