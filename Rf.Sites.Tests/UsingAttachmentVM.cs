using NUnit.Framework;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Models.Extender;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class UsingAttachmentVM
  {
    private static Environment env;
    private static IVmExtender<AttachmentVM>[] extender;

    [TestFixtureSetUp]
    public void Setup()
    {
      env = new Environment {DropZoneUrl = "files"};
      extender = new[] {new AttachmentPathModifier(env)};
    }

    [Test]
    public void AttachmentVMShowsSizeInKilobytes()
    {
      var aVM = new AttachmentVM(new Attachment { Size = 2048 + 512 }, null);
      aVM.Size.ShouldBeEqualTo("2.5");
      aVM.SizeDimension.ShouldBeEqualTo("KB");
    }

    [Test]
    public void BiggerAttachmentsAreShownInMegs()
    {
      var aVM = new AttachmentVM(new Attachment { Size = 2 * 1024 * 1024 + 50000 }, null);
      aVM.Size.ShouldBeEqualTo("2.05");
      aVM.SizeDimension.ShouldBeEqualTo("MB");
    }

    [Test]
    public void AttachmentExtenderAddsSlashPrefix()
    {
      var aVM = new AttachmentVM(new Attachment {Path = "files/h.txt"}, extender);
      aVM.Path.ShouldBeEqualTo("/files/h.txt");
    }

    [Test]
    public void AttachmentExtenderAddsDropZonePrefix()
    {
      var aVM = new AttachmentVM(new Attachment { Path = "h.txt" }, extender);
      aVM.Path.ShouldBeEqualTo("/files/h.txt");
    }

    [Test]
    public void AttachmentExtenderDoesNotConfuseSlashes()
    {
      var aVM = new AttachmentVM(new Attachment { Path = "/h.txt" }, extender);
      aVM.Path.ShouldBeEqualTo("/files/h.txt");
    }
    
  }
}