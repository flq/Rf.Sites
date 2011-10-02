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
            var vm = new ContentVM(next.Model, null, null, null, null);
            var jsonTitle = vm.JsonTitle;
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


        [TearDown]
        public void Reset()
        {
            Repository.Clear();
        }
    }
}