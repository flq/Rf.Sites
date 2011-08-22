using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Rf.Sites.Features.DisplayContent;
using Rf.Sites.Test.Support;
using FluentAssertions;

namespace Rf.Sites.Test
{
    [TestFixture]
    public class CodeHighlightTests
    {
        private readonly CodeHighlightExtension _code = new CodeHighlightExtension();

        [Test]
        public void DefaultBehaviourReplacesCodeForCSharpDisplay()
        {
            var cVm = GetContentVm(DataMother.GetContentSampleBody1());

            _code.Inspect(cVm);

            cVm.Body.Contains("<pre name=\"code\"").Should().BeTrue("pre tag should be modified");
            cVm.NeedsCodeHighlighting.Should().BeTrue("this body needs code highlighting");
        }

        [Test]
        public void NoCodeMeansNoHighlightNeeded()
        {
            var cVm = GetContentVm("<p>Hello</p>");
            _code.Inspect(cVm);

            cVm.NeedsCodeHighlighting.Should().BeFalse("No code highlight needed");
        }

        [Test]
        public void AngleBracketsAreEscapedInsideCodeTags()
        {
            var codeBlock =
                CodeHighlightExtension.RepareAngleBrackets(DataMother.GetContentWithBracketCode())
                .Substring(2443, 91);
            
            codeBlock.Contains("<").Should().BeFalse();
            codeBlock.Contains("&lt;").Should().BeTrue();
            codeBlock.Contains(">").Should().BeFalse();
            codeBlock.Contains("&gt;").Should().BeTrue();
        }

        [Test]
        public void SingleLineIssue()
        {
            var s = DataMother.SecndContentWithBracketCode();
            s = CodeHighlightExtension.RepareAngleBrackets(s);
            var start = new Regex(@"\<pre").Matches(s).Count;
            var end = new Regex(@"\</pre").Matches(s).Count;
            start.Should().Be(end);
        }

        [Test]
        public void SpeedCheckEscaping()
        {
            var s = DataMother.GetContentWithBracketCode();

            new Action(() =>
                           {
                               for (int i = 0; i < 1000; i++)
                               {
                                   CodeHighlightExtension.RepareAngleBrackets(s);
                               }
                           })
                           .ExecutionTime()
                           .ShouldNotExceed(TimeSpan.FromMilliseconds(100));
        }


        private static ContentVM GetContentVm(string body)
        {
            return new ContentVM(null, null) { Body = body };
        }
    }
}