using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
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

      cVM.Body.Contains("<pre name=\"code\"").ShouldBeTrue();
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

    [Test]
    public void AngleBracketsAreEscapedInsideCodeTags()
    {
      var s = DataMother.GetContentWithBracketCode();

      CodeBracketReparer.ApplyChanges(ref s);

      var codeBlock = s.Substring(2443, 91);
      codeBlock.Contains("<").ShouldBeFalse();
      codeBlock.Contains("&lt;").ShouldBeTrue();
      codeBlock.Contains(">").ShouldBeFalse();
      codeBlock.Contains("&gt;").ShouldBeTrue();

    }

    [Test]
    public void SpeedCheckEscaping()
    {
      var s = DataMother.GetContentWithBracketCode();
      var sw = Stopwatch.StartNew();
      for (int i = 0; i < 1000; i++)
      {
        var x = s;
        CodeBracketReparer.ApplyChanges(ref x);
      }
      Console.WriteLine("Total: {0}, per op: {1}", sw.ElapsedMilliseconds, sw.ElapsedMilliseconds / (float)1000);
    }

    [Test]
    public void SingleLineIssue()
    {
      var s = DataMother.SecndContentWithBracketCode();
      CodeBracketReparer.ApplyChanges(ref s);
      var start = new Regex(@"\<pre").Matches(s).Count;
      var end = new Regex(@"\</pre").Matches(s).Count;
      start.ShouldBeEqualTo(end);
    }
  }
}