using FluentAssertions;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Features;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame;
using Rf.Sites.Frame.SiteInfrastructure;
using Rf.Sites.Test.DataScenarios;

namespace Rf.Sites.Test
{
    [TestFixture]
    internal class Using_single_content : In_memory_repository_context<Content>
    {
        readonly SingleContent _sc;

        public Using_single_content()
        {
            _sc = new SingleContent(Repository);
        }

        [Test]
        public void default_model_for_no_content_is_404()
        {
            var next = _sc.GetContent(13);
            next.TransferRequired.Should().BeTrue();
            next.TransferInputModel.Should().BeAssignableTo<InputModel404>();
        }

        [Test]
        public void found_content_is_available()
        {
            ApplyData<SomeContent>();
            var next = _sc.GetContent(1);
            next.TransferRequired.Should().BeFalse();
            next.Model.Should().NotBeNull();
        }

        [Test]
        public void title_is_available_for_javascript_var()
        {
            ApplyData<FunnyTitle>();
            var next = _sc.GetContent(1);
            var vm = new ContentVM(next.Model, new SiteSettings(), null, null);
            var jsonTitle = vm.CommentData.Title;
            jsonTitle.Should().Contain("\\u0027");
            jsonTitle.Should().Contain("\\\"");
        }


        [Test]
        public void future_content_is_transferred_to_new_model()
        {
            ApplyData<SomeFutureContent>();
            var next = _sc.GetContent(1);
            next.TransferRequired.Should().BeTrue();
            next.TransferInputModel.Should().BeAssignableTo<NotYetPublishedVM>();
        }

        [Test]
        public void markdown_filter_is_applied()
        {
            ApplyData<MarkdownContent>();
            var next = _sc.GetContent(1);
            next.TransferRequired.Should().BeFalse();
            next.Model.Body.Should().Contain("<h1>Hello World</h1>");
        }

        [Test]
        public void code_display_will_work_correctly()
        {
            ApplyData<MarkdownContentWithFSharpCode>();
            var next = _sc.GetContent(1);
            var vm = GetContentVm(next.Model);
        }

        [Test]
        public void content_continuation_pipeline_only_once()
        {
            int i = 0;
            var c = new ContentContinuation<object, ContentVM>(new object()).AddPipeline(o => { i++; return o; });
            var _ = c.TransferRequired;
            i.Should().Be(1);
            var __ = c.Model;
            i.Should().Be(1);
        }

        [TearDown]
        public void Reset()
        {
            Repository.Clear();
        }
    }
}