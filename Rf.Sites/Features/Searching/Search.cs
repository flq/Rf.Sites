using System;
using System.Collections.Generic;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;
using System.Linq;

namespace Rf.Sites.Features.Searching
{
    [HasActions]
    public class Search
    {
        private readonly Func<IRepository<Content>> _contentFactory;
        private readonly Func<IRepository<Tag>> _tagFactory;
        private readonly ICache _cache;
        private readonly string _contentLinkTemplate;

        public Search(Func<IRepository<Content>> contentFactory, Func<IRepository<Tag>> tagFactory, ICache cache, IUrlRegistry urlRegistry)
        {
            _contentFactory = contentFactory;
            _tagFactory = tagFactory;
            _cache = cache;
            _contentLinkTemplate = urlRegistry.TemplateFor(new ContentId());
            EnsureCached();
        }

        [UrlPattern("lookup")]
        public IJsonResponse Lookup(SearchTerm search)
        {
            var titleResult = SearchTitles(search.term);
            return new SearchTextResponse(titleResult);
        }

        private IEnumerable<SearchResult> SearchTitles(string term)
        {
            return from title in _cache.Get<List<Tuple<int,string>>>("titles")
                   where title.Item2.StartsWith(term, StringComparison.InvariantCultureIgnoreCase)
                   select new SearchResult
                              {
                                  prefixtext = "Post", 
                                  values = new[]
                                               {
                                                   new Link
                                                       {
                                                           linktext = title.Item2, 
                                                           link = _contentLinkTemplate.Replace("{0}", title.Item1.ToString())
                                                       }
                                               }
                              };
        }

        private void EnsureCached()
        {
            if (TitlesAreNotCached)
                CacheTitles();
            if (TagsAreNotCached)
                CacheTags();
        }

        protected bool TitlesAreNotCached
        {
            get { return !_cache.HasValue("titles"); }
        }

        protected bool TagsAreNotCached
        {
            get { return !_cache.HasValue("tags"); }
        }

        private void CacheTitles()
        {
            var rep = _contentFactory();
            var titles = rep.Select(r => Tuple.Create(r.Id, r.Title)).OrderBy(t => t.Item2).ToList();
            _cache.Add("titles", titles);
        }

        private void CacheTags()
        {
            var rep = _tagFactory();
            var tags = rep.Select(r => r.Name).OrderBy(t => t).ToList();
            _cache.Add("tags", tags);
        }
    }
}