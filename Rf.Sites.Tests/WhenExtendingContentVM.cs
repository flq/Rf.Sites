using Moq;
using NUnit.Framework;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Tests.Support;

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
    }
  }
}