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
    public class SearchOnPosts : ISearchPlugin
    {
        private readonly ICache _cache;
        private readonly IUrlRegistry _urls;
        private readonly Func<IRepository<Content>> _contentFactory;

        public SearchOnPosts(ICache cache, IUrlRegistry urls, Func<IRepository<Content>> contentFactory)
        {
            _cache = cache;
            _urls = urls;
            _contentFactory = contentFactory;
        }

        public IEnumerable<Link> LinksFor(string searchTerm)
        {
            if (TitlesAreNotCached)
                CacheTitles();
            return from title in _cache.Get<List<Tuple<int, string>>>("titles")
                   where title.Item2.StartsWith(searchTerm, StringComparison.InvariantCultureIgnoreCase)
                   select new Link
                              {
                                  linktext = title.Item2,
                                  link = _urls.UrlFor(new ContentId(title.Item1))
                              };
        }

        private void CacheTitles()
        {
            var rep = _contentFactory();
            var titles = rep.Select(r => new { r.Id, r.Title }).OrderBy(r => r.Title)
                .ToList()
                .Select(r => Tuple.Create(r.Id, r.Title)).ToList();
            _cache.Add("titles", titles);
        }

        private bool TitlesAreNotCached
        {
            get { return !_cache.HasValue("titles"); }
        }
    }
}