using System;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;
using System.Linq;

namespace Rf.Sites.Features
{
    public class FeedProvisionEndpoint
    {
        private readonly IRepository<Content> _contents;
        private readonly FeedSetup _feedSetup;
        private readonly int _itemsToTake;

        public FeedProvisionEndpoint(IRepository<Content> contents, IUrlRegistry registry, ServerVariables vars, PaginationSettings pagination, SiteSettings site)
        {
            _contents = contents;
            _itemsToTake = pagination.ItemsPerFeed;
            _feedSetup = new FeedSetup
                             {
                                 SiteMasterName = site.SiteAuthor,
                                 Title = site.SiteTitle,
                                 CopyrightNotice = site.SiteCopyright,
                                 UrlTemplate = registry.BuildAbsoluteUrlTemplate(vars, r => r.TemplateFor(new ContentId()))
                             };
        }

        public FeedModel RssFeed(InputFeedTag tag)
        {
            _feedSetup.FeedType = FeedType.Rss;
            return ContentFeed(tag.Tag);
        }

        public FeedModel AtomFeed(InputFeedTag tag)
        {
            _feedSetup.FeedType = FeedType.Atom;
            return ContentFeed(tag.Tag);
        }

        private FeedModel ContentFeed(string tag)
        {
            AppendSubtitle(tag);
            _feedSetup.Content = GetQueryable(tag)
                .Select(c => new ContentTeaserVM(c.Id, c.Title, c.Created, c.Teaser))
                .Take(_itemsToTake)
                .ToList();
            return new FeedModel(_feedSetup);
        }

        private IQueryable<Content> GetQueryable(string tag)
        {
            var query = tag != "all" ?
                _contents.Where(c => c.Created < DateTime.Now.ToUniversalTime() && c.Tags.Any(t => t.Name == tag)) :
                _contents.Where(c => c.Created < DateTime.Now.ToUniversalTime());
            return query.OrderByDescending(c => c.Created);
        }

        private void AppendSubtitle(string tag)
        {
            _feedSetup.AppendSubtitle("Latest Content" + (tag == "all" ? "" : (" in " + tag)));
        }
    }

    public class InputFeedTag
    {
        [RouteInput("all")]
        public string Tag { get; set; }
    }
}