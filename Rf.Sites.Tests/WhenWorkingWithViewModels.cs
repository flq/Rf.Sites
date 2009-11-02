using Moq;
using NUnit.Framework;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Models.Extender;
using Rf.Sites.Tests.Frame;
using System.Linq;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class WhenWorkingWithViewModels
  {
    private const string checkMail = "boobles@shambocks.com";
    private const string expectedUrl = "http://www.gravatar.com/avatar/18d0aad273d654dfece3952113268428";

    [Test]
    public void GravatarUrlIsFormedCorrectly()
    {
      var c = new Comment { Body = "Hi", CommenterEmail = checkMail };
      var cVM = new CommentVM(c);
      cVM.GravatarImageSource.StartsWith(expectedUrl).ShouldBeTrue();
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

      var vm = new ContentViewModel(cnt, null, cvt, null);
      
      vm.Comments.ShouldNotBeNull();
      vm.Comments.Count().ShouldBeEqualTo(1);
      e.Verify(x=>x.Inspect(It.IsAny<CommentVM>()));
    }

    [Test]
    public void ContentVMWillPropagateAttachmentConstruction()
    {
      var cnt = new Content { Body = "Hello World" };
      cnt.AddAttachment(new Attachment { Name = "Great Attachment" });

      var vm = new ContentViewModel(cnt,null);

      vm.Attachments.ShouldNotBeNull();
      vm.Attachments.Count().ShouldBeEqualTo(1);
    }

    [Test]
    public void SiteManagerCommentInspectorWorksAsIntended()
    {
      var aWebsite = "http://lefleur";
      var e = new CommentFromSiteMasterExtension(
        new Environment {SiteMasterEmail = checkMail, SiteMasterWebPage = aWebsite});

      var cmtVM = new CommentVM(new Comment { IsFromSiteMaster = true});
      e.Inspect(cmtVM);
      cmtVM.GravatarImageSource.StartsWith(expectedUrl).ShouldBeTrue();
      cmtVM.Website.ShouldBeEqualTo(aWebsite);
    }
    
    [Test]
    public void AttachmentVMShowsSizeInKilobytes()
    {
      var aVM = new AttachmentVM(new Attachment {Size = 2048 + 512});
      aVM.Size.ShouldBeEqualTo("2.5");
      aVM.SizeDimension.ShouldBeEqualTo("KB");
    }

    [Test]
    public void BiggerAttachmentsAreShownInMegs()
    {
      var aVM = new AttachmentVM(new Attachment { Size = 2 * 1024 * 1024 + 50000 });
      aVM.Size.ShouldBeEqualTo("2.05");
      aVM.SizeDimension.ShouldBeEqualTo("MB");
    }
  }
}