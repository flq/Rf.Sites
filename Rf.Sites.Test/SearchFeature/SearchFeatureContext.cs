using System;
using System.Collections.Generic;
using FubuMVC.Core.Urls;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Features.Searching;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;
using Rf.Sites.Test.Frame;
using FluentAssertions;
using System.Linq;
using Rf.Sites.Test.Support;

namespace Rf.Sites.Test.SearchFeature
{
    public class SearchFeatureContext
    {
        private SearchEndpoint _search;
        private InMemoryCache _cache;
        protected bool ContentFactoryWasCalled;
        protected bool TagFactoryWasCalled;
        private IJsonResponse _response;
        private IUrlRegistry _urlRegistry;
        private DbTestContext _dbContext;

        [TestFixtureSetUp]
        public void Given()
        {
            _cache = new InMemoryCache();
            _urlRegistry = new TestUrlRegistry();
            _dbContext = new DbTestContext();
            Setup();
            _search = new SearchEndpoint(new ISearchPlugin[] {
                new SearchOnPosts(_cache, _urlRegistry, GetContentFactory), 
                new SearchOnTags(_cache, _urlRegistry, GetTagFactory),
                new SearchOnTime(_cache, _urlRegistry, ()=>DBContext.Session)
            });
        }

        protected virtual void Setup()
        {
            
        }

        protected DbTestContext DBContext
        {
            get { return _dbContext; }
        }

        protected void Search(string term)
        {
            _response = _search.Lookup(new SearchTerm { term = term });
        }

        protected void SearchReturnedThisLink(string title, string link)
        {
            _response.Should().NotBeNull();
            var found = ((IEnumerable<Link>)_response).FirstOrDefault(v => v.linktext.Equals(title));
            found.Should().NotBeNull("link with text " + title + " should exist");
            found.linktext.Should().Be(title);
            found.link.Should().Be(link);
        }

        protected void SearchReturnedNothing()
        {
            _response.Should().NotBeNull();
            var count = ((IEnumerable<Link>)_response).Count();
            count.Should().Be(0);
        }

        protected void SearchReturnedTags(params string[] tags)
        {
            _response.Should().NotBeNull();
            var links = (IEnumerable<Link>)_response;
            foreach (var t in tags)
            {
                links.Select(l => l.linktext).Contains("Content from " + t).Should().BeTrue("Tag " + t + " is contained");
            }
        }

        protected void CachedTitlesAre(params string[] cachedTitles)
        {
            if (!_cache.HasValue("titles"))
                Assert.Fail("Titles have not been cached");
            var titles = _cache.Get<List<Tuple<int, string>>>("titles");
            titles.Select(t => t.Item2).Should().BeEquivalentTo(cachedTitles);
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
            _cache.Add("titles", new List<Tuple<int,string>>(titles.Select((s,i)=> Tuple.Create(i, s))));
        }

        protected void AddTagsToCache(params string[] tags)
        {
            _cache.Add("tags", new List<string>(tags));
        }

        protected IRepository<Tag> TagFactoryWithTags(params string[] tags)
        {
            var em = new EntityMaker();
            var rep = new InMemoryRepository<Tag>();
            foreach (var t in tags)
                rep.Add(em.CreateTag(t));
            return rep;
        }

        protected IRepository<Content> ContentFactoryWithTitles(params string[] titles)
        {
            var em = new EntityMaker();
            var rep = new InMemoryRepository<Content>();
            foreach (var t in titles)
                rep.Add(em.CreateContent(t));
            return rep;
        }

        protected void ClearCache()
        {
            _cache.Clear();
        }
    }
}