using System;
using System.Linq;
using NUnit.Framework;
using Rf.Sites.Features;
using Rf.Sites.Features.Models;
using Rf.Sites.Test.DataScenarios;
using Rf.Sites.Test.Frame;
using FluentAssertions;

namespace Rf.Sites.Test
{
    [TestFixture]
    internal class Pagination_total_count_update : Pagination_context
    {

        [TestFixtureSetUp]
        public void Given()
        {
            ApplyData<SomeContent>();
            SetQuery(from c in Repository select new ContentTeaserVM(c.Id, c.Title, c.Created, c.Teaser));
            SetPagingArgs(new PagingArgs { Page = 1 });
            InvokeBehavior();
        }

        [Test]
        public void the_current_page_is_zero()
        {
            Page.CurrentPage.Should().Be(1);
        }

        [Test]
        public void the_total_count_is_one()
        {
            Page.TotalCount.Should().Be(1);
        }
    }

    [TestFixture]
    internal class Pagination_query_execution : Pagination_context
    {
        [TestFixtureSetUp]
        public void Given()
        {
            ApplyData<Content_10>();
            SetQuery(from c in Repository select new ContentTeaserVM(c.Id, c.Title, c.Created, c.Teaser));
            SetPagingArgs(new PagingArgs { Page = 1 });
            SetItemsPerPage(2);
            InvokeBehavior();
        }

        [Test]
        public void got_the_second_page()
        {
            Page.Elements[0].Id.Should().Be(2);
        }

        [Test]
        public void got_two_elements()
        {
            Page.Elements.Should().HaveCount(2);
        }
    }
}