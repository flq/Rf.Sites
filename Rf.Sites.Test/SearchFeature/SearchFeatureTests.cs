using System;
using NUnit.Framework;
using FluentAssertions;

namespace Rf.Sites.Test.SearchFeature
{
    [TestFixture]
    public class FilledCacheUsageInit : SearchFeatureContext
    {
        protected override void Setup()
        {
            AddTagsToCache();
            AddTitlesToCache();
        }

        [Test]
        public void content_repository_was_not_called()
        {
            ContentFactoryWasCalled.Should().BeFalse();
        }

        [Test]
        public void tag_repository_was_not_called()
        {
            TagFactoryWasCalled.Should().BeFalse();
        }
    }

    public class SearchOperations : SearchFeatureContext
    {
        protected override void Setup()
        {
            AddTitlesToCache("A view to a kill", "Descriptive measures", "Reassuring evidence", "Colossal uUtcome", "Crisp Shit", "Wild guesses from insanity");
            AddTagsToCache("wcf", "wpf", "programming");
        }

        [Test]
        public void case_insensitive_search()
        {
            Search("a");
            SearchReturnedPost("A view to a kill", "/go/0");
        }

        [Test]
        public void tag_search()
        {
            Search("pr");
            SearchReturnedTags("programming");
        }

        [Test]
        public void search_returns_tags_and_titles()
        {
            Search("w");
            SearchReturnedTags("wcf", "wpf");
            SearchReturnedPost("Wild guesses from insanity", "/go/5");
        }
    }
}