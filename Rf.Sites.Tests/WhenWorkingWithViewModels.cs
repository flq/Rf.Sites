using Moq;
using NUnit.Framework;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Tests.Frame;
using System.Linq;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class WhenWorkingWithViewModels
  {
    private const string checkMail = "boobles@shambocks.com";
    private const string expectedUrl = "http://www.gravatar.com/avatar/18d0aad273d654dfece3952113268428?s=80&d=identicon";

    [Test]
    public void GravatarUrlIsFormedCorrectly()
    {
      var c = new Comment { Body = "Hi", CommenterEmail = checkMail };

      var cVM = new CommentVM(c);

      cVM.GravatarImageSource.ShouldBeEqualTo(expectedUrl);
    }

    [Test]
    public void GravatarIsBuiltWithoutEmail()
    {
      var c = new Comment { Body = "Hi" };
      var cVM = new CommentVM(c);
      cVM.GravatarImageSource.ShouldNotBeNull();
    }

    [Test]
    public void ExtenderAreAppliedToComments()
    {
      var e = new Mock<IVmExtender<CommentVM>>();
      var c = new CommentVM(new Comment {Body = "Hi"}, new[] {e.Object});
      e.Verify(x=>x.Inspect(c));
    }

    [Test]
    public void ContentVMWillPropagateCommentConstruction()
    {
      var cnt = new Content {Body = "Hello World"};
      cnt.AddComment(new Comment { Body = "Great content" });

      var e = new Mock<IVmExtender<CommentVM>>();
      var cvt = ObjectConverter.From((Comment c) => new CommentVM(c, new[] {e.Object}));

      var vm = new ContentViewModel(cnt, null, cvt);
      
      vm.Comments.ShouldNotBeNull();
      vm.Comments.Count().ShouldBeEqualTo(1);
      e.Verify(x=>x.Inspect(It.IsAny<CommentVM>()));
    }
  }
}