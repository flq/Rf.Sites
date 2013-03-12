using FluentAssertions;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Features;
using Rf.Sites.Features.Models;
using Rf.Sites.Test.DataScenarios;

namespace Rf.Sites.Test
{
    [TestFixture]
    internal class Using_single_content : In_memory_repository_context<Content>
    {
        readonly SingleContentEndpoint _sc;

        public Using_single_content()
        {
            _sc = new SingleContentEndpoint(Repository,SiteSettings,ServerVariables,Reg);
        }

        [Test]
        public void default_model_for_no_content_is_null()
        {
            var next = _sc.GetContent(new ContentId(13));
            next.Should().BeNull();
        }

        [Test]
        public void found_content_is_available()
        {
            ApplyData<SomeContent>();
            var next = _sc.GetContent(new ContentId(1));
            next.Should().NotBeNull();
            next.ContentId.Should().Be("1");
        }

        [Test]
        public void title_is_available_for_javascript_var()
        {
            ApplyData<FunnyTitle>();
            var next = _sc.GetContent(new ContentId(1));
            var jsonTitle = next.CommentData.Title;
            jsonTitle.Should().Contain("\\u0027");
            jsonTitle.Should().Contain("\\\"");
        }

        [Test]
        public void markdown_filter_is_applied()
        {
            ApplyData<MarkdownContent>();
            var next = _sc.GetContent(new ContentId(1));
            next.Body.Should().Contain("<h1>Hello World</h1>");
        }

        [Test]
        public void code_display_will_work_correctly()
        {
            ApplyData<MarkdownContentWithFSharpCode>();
            var next = _sc.GetContent(new ContentId(1));
        }

        [TearDown]
        public void Reset()
        {
            Repository.Clear();
        }
    }
}