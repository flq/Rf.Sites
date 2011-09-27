using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Urls;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Searching
{
    public class SearchOnTags : ISearchPlugin
    {
        private readonly ICache _cache;
        private readonly IUrlRegistry _urls;
        private readonly Func<IRepository<Tag>> _tagFactory;

        public SearchOnTags(ICache cache, IUrlRegistry urls, Func<IRepository<Tag>> tagFactory)
        {
            _cache = cache;
            _urls = urls;
            _tagFactory = tagFactory;
        }

        public IEnumerable<Link> LinksFor(string searchTerm)
        {
            if (TagsAreNotCached)
                CacheTags();

            return from title in _cache.Get<List<string>>("tags")
                   where title.StartsWith(searchTerm, StringComparison.InvariantCultureIgnoreCase)
                   select new Link
                              {
                                  linktext = "Content from " + title,
                                  link = _urls.UrlFor(new TagPaging(title))
                              };
        }

        private void CacheTags()
        {
            var rep = _tagFactory();
            var tags = rep.Select(r => r.Name).OrderBy(t => t).ToList();
            _cache.Add("tags", tags);
        }

        private bool TagsAreNotCached
        {
            get { return !_cache.HasValue("tags"); }
        }
    }
}