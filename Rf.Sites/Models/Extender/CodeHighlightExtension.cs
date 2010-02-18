using System;
using Rf.Sites.Frame;

namespace Rf.Sites.Models.Extender
{
  [Order(10)]
  public class CodeHighlightExtension : IVmExtender<CommentVM>, IVmExtender<ContentViewModel>
  {
    public const string CsharpPreTag = "<pre class=\"sh_csharp\">";

    public void Inspect(ContentViewModel viewModel)
    {
      //Some posts may have been published wit the pre tag already in it:
      //In this case I assume that all instances have been changed.
      if (viewModel.Body.Contains(CsharpPreTag))
      {
        viewModel.NeedsCodeHighlighting = true;
        return;
      }
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
      changesApplied |= getBody(ref body, "code");
      changesApplied |= getBody(ref body, "csharp");
      changesApplied |= getBody(ref body, "xmlcode");

      return changesApplied;
    }

    private static bool getBody(ref string body, string checkTag)
    {
      var changes = false;
      var openTag = "<" + checkTag + ">";
      var closeTag = "</" + checkTag + ">";

      if (body.Contains(openTag))
      {
        changes = true;
        body = body.Replace(openTag, CsharpPreTag);
        body = body.Replace(closeTag, "</pre>");
      }
      return changes;
    }
  }
}