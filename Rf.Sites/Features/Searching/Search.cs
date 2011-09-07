using System;
using FubuMVC.Core;
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

        public Search(Func<IRepository<Content>> contentFactory, Func<IRepository<Tag>> tagFactory, ICache cache)
        {
            _contentFactory = contentFactory;
            _tagFactory = tagFactory;
            _cache = cache;
            EnsureCached();
        }

        [UrlPattern("lookup")]
        public IJsonResponse Lookup(SearchTerm search)
        {
            return new SearchTextResponse();
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
            var titles = rep.Select(r => r.Title).OrderBy(t => t).ToList();
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