using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Rf.Sites.Frame;

namespace Rf.Sites.Models.Extender
{
  [Order(60)]
  public class CodeBracketReparer : IVmExtender<CommentVM>, IVmExtender<ContentViewModel>
  {
    
    public void Inspect(CommentVM viewModel)
    {
      var s = viewModel.Body;
      ApplyChanges(ref s);
      viewModel.Body = s;
    }

    public void Inspect(ContentViewModel viewModel)
    {
      if (!viewModel.NeedsCodeHighlighting)
        return;

      var s = viewModel.Body;
      ApplyChanges(ref s);
      viewModel.Body = s;
    }

    public static void ApplyChanges(ref string body)
    {
      
      var sb = new StringBuilder();
      new ReplacedStringGen(body).ForEach(s=>sb.Append(s));
      body = sb.ToString();
      
    }

    private class ReplacedStringGen : IEnumerable<string>
    {
      private readonly string text;
      private int currentIndex;

      public ReplacedStringGen(string text)
      {
        this.text = text;
      }

      public void ForEach(Action<string> action)
      {
        foreach (var s in this)
          action(s);
      }

      public IEnumerator<string> GetEnumerator()
      {
        while (currentIndex != -1)
        {
          var indexOf = text.IndexOf(CodeHighlightExtension.CsharpStartTag, currentIndex);
          if (indexOf == -1)
          {
            yield return text.Substring(currentIndex);
            currentIndex = -1;
          }
          else
          {
            yield return text.Substring(currentIndex, indexOf - currentIndex);
            yield return CodeHighlightExtension.CsharpStartTag;
            var end = text.IndexOf("</pre>", indexOf);
            var tmp = text.Substring(indexOf + CodeHighlightExtension.CsharpStartTag.Length, end - indexOf - CodeHighlightExtension.CsharpStartTag.Length);
            tmp = tmp.Replace("<", "&lt;");
            tmp = tmp.Replace(">", "&gt;");
            yield return tmp;
            currentIndex = end;
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