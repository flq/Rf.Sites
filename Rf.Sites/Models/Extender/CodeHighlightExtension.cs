using System;
using Rf.Sites.Frame;

namespace Rf.Sites.Models.Extender
{
  public class CodeHighlightExtension : IVmExtender<ContentViewModel>, IVmExtender<CommentVM>
  {
    private const string csharp = "<pre class=\"sh_csharp\">";

    public void Inspect(ContentViewModel viewModel)
    {
      string body = viewModel.Body;
      viewModel.NeedsCodeHighlighting = applyChanges(ref body);
      viewModel.Body = body;
    }

    public void Inspect(CommentVM viewModel)
    {
      string body = viewModel.Body;
      applyChanges(ref body);
      viewModel.Body = body;
    }

    private static bool applyChanges(ref string body)
    {
      bool changesApplied = false;
      if (body.Contains("<code>"))
      {
        changesApplied = true;
        body = body.Replace("<code>", csharp);
        body = body.Replace("</code>", "</pre>");
      }
      if (body.Contains("<csharp>"))
      {
        changesApplied = true;
        body = body.Replace("<csharp>", csharp);
        body = body.Replace("</csharp>", "</pre>");
      }
      return changesApplied;
    }
  }
}