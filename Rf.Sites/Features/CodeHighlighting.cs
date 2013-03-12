using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Features
{
    public class CodeHighlightExtension
    {
        private const string CsharpEndTag = "</pre>";
        public const string CsharpStartTag = "<pre name=\"code\" class=\"c#\">";

        public void Inspect(ContentVM viewModel)
        {
            //Some posts may have been published wit the pre tag already in it:
            //In this case I assume that all instances have been changed.
            if (viewModel.Body.Contains(CsharpStartTag))
            {
                viewModel.NeedsCodeHighlighting = true;
                return;
            }
            var body = viewModel.Body;
            viewModel.NeedsCodeHighlighting = applyChanges(ref body);
            viewModel.Body = body;
        }

        private static bool applyChanges(ref string body)
        {
            bool changesApplied = false;
            changesApplied |= getBody(ref body, "code");
            changesApplied |= getBody(ref body, "csharp");
            changesApplied |= getBody(ref body, "xmlcode");

            if (changesApplied)
                body = RepareAngleBrackets(body);

            return changesApplied;
        }

        public static string RepareAngleBrackets(string body)
        {
            var sb = new StringBuilder();
            new ReplacedStringGen(body).ForEach(s => sb.Append(s));
            body = sb.ToString();
            return body;
        }

        private static bool getBody(ref string body, string checkTag)
        {
            var changes = false;
            var openTag = "<" + checkTag + ">";
            var closeTag = "</" + checkTag + ">";

            if (body.Contains(openTag))
            {
                changes = true;
                body = body.Replace(openTag, CsharpStartTag);
                body = body.Replace(closeTag, CsharpEndTag);
            }
            return changes;
        }

        private class ReplacedStringGen : IEnumerable<string>
        {
            private readonly string _text;
            private int _currentIndex;

            public ReplacedStringGen(string text)
            {
                _text = text;
            }

            public void ForEach(Action<string> action)
            {
                foreach (var s in this)
                    action(s);
            }

            public IEnumerator<string> GetEnumerator()
            {
                while (_currentIndex != -1)
                {
                    var indexOf = _text.IndexOf(CsharpStartTag, _currentIndex, StringComparison.Ordinal);
                    if (indexOf == -1)
                    {
                        yield return _text.Substring(_currentIndex);
                        _currentIndex = -1;
                    }
                    else
                    {
                        yield return _text.Substring(_currentIndex, indexOf - _currentIndex);
                        yield return CsharpStartTag;
                        var end = _text.IndexOf("</pre>", indexOf, StringComparison.Ordinal);
                        var tmp = _text.Substring(indexOf + CsharpStartTag.Length, end - indexOf - CsharpStartTag.Length);
                        tmp = tmp.Replace("<", "&lt;");
                        tmp = tmp.Replace(">", "&gt;");
                        yield return tmp;
                        _currentIndex = end;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}