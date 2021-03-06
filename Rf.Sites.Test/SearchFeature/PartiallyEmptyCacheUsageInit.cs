﻿using FluentAssertions;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Frame.Persistence;

namespace Rf.Sites.Test.SearchFeature
{
    public class PartiallyEmptyCacheUsageInit : SearchFeatureContext
    {
        protected override void Setup()
        {
            AddTagsToCache("A");
        }

        protected override IRepository<Content> GetContentFactory()
        {
            ContentFactoryWasCalled = true;
            return ContentFactoryWithTitles("A", "B");
        }

        [Test]
        public void content_repository_was_called()
        {
            Search("x");
            ContentFactoryWasCalled.Should().BeTrue();
        }

        [Test]
        public void titles_are_cached()
        {
            CachedTitlesAre("A", "B");
        }

        [Test]
        public void tag_repository_was_not_called()
        {
            TagFactoryWasCalled.Should().BeFalse();
        }
    }
}