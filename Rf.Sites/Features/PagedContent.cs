using System;
using System.Linq;
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

        public ContentTeaserPage Year(YearPaging paging)
        {
            var lower = new DateTime(paging.Year, 1, 1);
            var upper = new DateTime(paging.Year, 12, 31);
            return new ContentTeaserPage(paging, GetBetweenQuery(lower, upper));
        }

        public ContentTeaserPage Month(MonthPaging paging)
        {
            var lower = new DateTime(paging.Year, SanitizeMonth(paging.Month), 1);
            var upper = lower.AddMonths(1);
            return new ContentTeaserPage(paging, GetBetweenQuery(lower,upper));
        }

        public ContentTeaserPage Tag(TagPaging tag)
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
            return month > 12 ? 12 : month;
        }
    }
}