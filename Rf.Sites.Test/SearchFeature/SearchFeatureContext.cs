using System.Collections.Generic;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Features.Searching;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;
using Rf.Sites.Test.Frame;
using FluentAssertions;

namespace Rf.Sites.Test.SearchFeature
{
    public class SearchFeatureContext
    {
        private Search _search;
        private InMemoryCache _cache;
        protected bool ContentFactoryWasCalled;
        protected bool TagFactoryWasCalled;

        [TestFixtureSetUp]
        public void Given()
        {
            _cache = new InMemoryCache();
            Setup();
            _search = new Search(GetContentFactory, GetTagFactory, _cache);
        }

        protected virtual void Setup()
        {
            
        }

        protected virtual IRepository<Content> GetContentFactory()
        {
            ContentFactoryWasCalled = true;
            return new InMemoryRepository<Content>();
        }

        protected virtual IRepository<Tag> GetTagFactory()
        {
            TagFactoryWasCalled = true;
            return new InMemoryRepository<Tag>();
        }

        protected void AddTitlesToCache(params string[] titles)
        {
            _cache.Add("titles", new List<string>(titles));
        }

        protected void AddTagsToCache(params string[] tags)
        {
            _cache.Add("tags", new List<string>(tags));
        }

        protected IRepository<Content> ContentFactoryWithTitles(params string[] titles)
        {
            var em = new EntityMaker();
            var rep = new InMemoryRepository<Content>();
            foreach (var t in titles)
                rep.Add(em.CreateContent(t));
            return rep;
        }

        protected IRepository<Tag> TagFactoryWithTags(params string[] tags)
        {
            var em = new EntityMaker();
            var rep = new InMemoryRepository<Tag>();
            foreach (var t in tags)
                rep.Add(em.CreateTag(t));
            return rep;
        }

        protected void CachedTitlesAre(params string[] cachedTitles)
        {
            if (!_cache.HasValue("titles"))
                Assert.Fail("Titles have not been cached");
            var titles = _cache.Get<List<string>>("titles");
            titles.Should().BeEquivalentTo(cachedTitles);
        }
    }
}