﻿using System;
using System.Collections.Generic;
using FubuMVC.Core.Urls;
using Moq;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Features.Searching;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;
using Rf.Sites.Test.Frame;
using FluentAssertions;
using System.Linq;

namespace Rf.Sites.Test.SearchFeature
{
    public class SearchFeatureContext
    {
        private Search _search;
        private InMemoryCache _cache;
        protected bool ContentFactoryWasCalled;
        protected bool TagFactoryWasCalled;
        private IJsonResponse _response;
        private Mock<IUrlRegistry> _urlRegistry;

        [TestFixtureSetUp]
        public void Given()
        {
            _cache = new InMemoryCache();
            Setup();
            _urlRegistry = new Mock<IUrlRegistry>();
            _urlRegistry.Setup(u => u.TemplateFor(It.IsAny<ContentId>())).Returns("/go/{0}");
            _urlRegistry.Setup(u => u.TemplateFor(It.IsAny<TagPaging>())).Returns("/tag/{0}");
            _search = new Search(GetContentFactory, GetTagFactory, _cache, _urlRegistry.Object);
        }

        protected virtual void Setup()
        {
            
        }

        protected void Search(string term)
        {
            _response = _search.Lookup(new SearchTerm { term = term });
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

        protected IRepository<Content> ContentFactoryWithTitles(params string[] titles)
        {
            var em = new EntityMaker();
            var rep = new InMemoryRepository<Content>();
            foreach (var t in titles)
                rep.Add(em.CreateContent(t));
            return rep;
        }

        protected void SearchReturnedPost(string title, string link)
        {
            _response.Should().NotBeNull();
            var found = ((IEnumerable<SearchResult>)_response).FirstOrDefault(sr => sr.values.Any(v => v.linktext.Equals(title)));
            found.Should().NotBeNull("link with text " + title + " should exist");
            var lnk = found.values.First(v => v.linktext.Equals(title));
            lnk.link.Should().Be(link);
        }

        protected void SearchReturnedTag(string tag)
        {
            _response.Should().NotBeNull();
            var found = ((IEnumerable<SearchResult>)_response).FirstOrDefault(sr => sr.values.Any(v => v.linktext.Equals(tag)));
            found.Should().NotBeNull("tag-link with text " + tag + " should exist");
        }

        protected void SearchReturnedTagGroupContaining(params string[] tags)
        {
            _response.Should().NotBeNull();
            var found = ((IEnumerable<SearchResult>)_response).FirstOrDefault(sr => sr.values.Any(v => v.linktext.Equals(tags.First())));
            found.values.Select(l => l.linktext).Should().BeEquivalentTo(tags);
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
            var titles = _cache.Get<List<Tuple<int,string>>>("titles");
            titles.Select(t => t.Item2).Should().BeEquivalentTo(cachedTitles);
        }
    }
}