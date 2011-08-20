using FluentAssertions;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Features.DisplayContent;

namespace Rf.Sites.Test
{
  [TestFixture]
  public class UsingAttachmentVM
  {

    [Test]
    public void AttachmentVMShowsSizeInKilobytes()
    {
      var aVM = new AttachmentVM(new Attachment { Size = 2048 + 512 });
      aVM.Size.Should().Be("2.5");
      aVM.SizeDimension.Should().Be("KB");
    }

    [Test]
    public void BiggerAttachmentsAreShownInMegs()
    {
      var aVM = new AttachmentVM(new Attachment { Size = 2 * 1024 * 1024 + 50000 });
      aVM.Size.Should().Be("2.05");
      aVM.SizeDimension.Should().Be("MB");
    }

    [Test]
    public void AttachmentExtenderAddsSlashPrefix()
    {
      var aVM = new AttachmentVM(new Attachment {Path = "h.txt"}) { DropZone = "files" };
      aVM.Path.Should().Be("/files/h.txt");
    }

    [Test]
    public void AttachmentExtenderAddsDropZonePrefix()
    { 
      var aVM = new AttachmentVM(new Attachment { Path = "h.txt" }) { DropZone = "/files"};
      aVM.Path.Should().Be("/files/h.txt");
    }

    [Test]
    public void AttachmentExtenderDoesNotConfuseSlashes()
    {
      var aVM = new AttachmentVM(new Attachment { Path = "/h.txt" }) { DropZone = "files" };
      aVM.Path.Should().Be("/files/h.txt");
    }
    
  }
}