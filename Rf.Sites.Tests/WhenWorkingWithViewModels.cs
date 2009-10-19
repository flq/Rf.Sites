using NUnit.Framework;
using Rf.Sites.Domain;
using Rf.Sites.Models;
using Rf.Sites.Tests.Frame;

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
  }
}