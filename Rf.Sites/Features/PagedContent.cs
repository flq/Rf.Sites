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

        [UrlPattern("all/{Page:0}")]
        public ContentTeaserPage GetContentDefault(PagingArgs paging)
        {
            var query = from c in _contentRepository
                        where c.Created < DateTime.Now.ToUniversalTime()
                        orderby c.Created descending
                        select new ContentTeaserVM(c.Id, c.Title, c.Created, c.Teaser);
            var page = new ContentTeaserPage(paging, query);
            return page;
        }

        [UrlPattern("year/{Year}/{Page:0}")]
        public ContentTeaserPage GetContentByYear(YearPaging paging)
        {
            var lower = new DateTime(paging.Year, 1, 1);
            var upper = new DateTime(paging.Year, 12, 31);
            return new ContentTeaserPage(paging, GetBetweenQuery(lower, upper));
        }

        [UrlPattern("month/{Year}/{Month}/{Page:0}")]
        public ContentTeaserPage GetContentByMonth(MonthPaging paging)
        {
            var lower = new DateTime(paging.Year, SanitizeMonth(paging.Month), 1);
            var upper = lower.AddMonths(1);
            return new ContentTeaserPage(paging, GetBetweenQuery(lower,upper));
        }

        [UrlPattern("tag/{Tag}/{Page}")]
        public ContentTeaserPage GetbyTag(InputTag tag)
        {
            var query = from c in _contentRepository
                        where c.Created < DateTime.Now.ToUniversalTime() && c.Tags.Any(t => t.Name == tag.Tag)
                        orderby c.Created descending
                        select new ContentTeaserVM(c.Id, c.Title, c.Created, c.Teaser);
            return new ContentTeaserPage(tag, query);
        }

        private IQueryable<ContentTeaserVM> GetBetweenQuery(DateTime lower, DateTime upper)
        {
            return from c in _contentRepository
                   where c.Created >= lower && c.Created <= upper && c.Created < DateTime.Now.ToUniversalTime()
                   orderby c.Created descending
                   select new ContentTeaserVM(c.Id, c.Title, c.Created, c.Teaser);
        }

        private static int SanitizeMonth(int month)
        {
            if (month < 1) return 1;
            if (month > 12) return 12;
            return month;
        }
    }
}