using Moq;
using NUnit.Framework;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Models.Extender;
using Rf.Sites.Tests.Support;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class WhenExtendingContentVM
  {
    [Test]
    public void ContentVMCallsProvidedExtender()
    {
      var extMock = new Mock<IVmExtender<ContentViewModel>>();
      
      var cnt = new Content();
      cnt.AssociateWithTag(new Tag());
      var cVM = new ContentViewModel(cnt, new [] { extMock.Object });

      extMock.Verify(ve => ve.Inspect(cVM));
    }

    [Test]
    public void DefaultBehaviourReplacesCodeForCSharpDisplay()
    {
      var c = new Content
                {
                  Body = DataMother.GetContentSampleBody1()
                };
      c.AssociateWithTag(new Tag());

      var cVM = new ContentViewModel(c, new[] { new CodeHighlightExtension() });

      cVM.Body.Contains("<pre class=\"sh_csharp\">").ShouldBeTrue();
      cVM.NeedsCodeHighlighting.ShouldBeTrue();
    }

    [Test]
    public void NoCodeMeansNoHighlightNeeded()
    {
      var c = new Content
      {
        Body = "<p>Hello</p>"
      };
      c.AssociateWithTag(new Tag());
      var cVM = new ContentViewModel(c, new[] { new CodeHighlightExtension() });
      cVM.NeedsCodeHighlighting.ShouldBeFalse();
    }
  }
}