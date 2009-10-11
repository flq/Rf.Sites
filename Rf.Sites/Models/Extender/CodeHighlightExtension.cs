using Rf.Sites.Frame;

namespace Rf.Sites.Models.Extender
{
  public class CodeHighlightExtension : IVmExtender<ContentViewModel>
  {
    private const string csharp = "<pre class=\"sh_csharp\">";

    public void Inspect(ContentViewModel viewModel)
    {
      string body = viewModel.Body;
      if (body.Contains("<code>"))
      {
        viewModel.NeedsCodeHighlighting = true;
        body = body.Replace("<code>", csharp);
        body = body.Replace("</code>", "</pre>");
      }
      if (body.Contains("<csharp>"))
      {
        viewModel.NeedsCodeHighlighting = true;
        body = body.Replace("<csharp>", csharp);
        body = body.Replace("</csharp>", "</pre>");
      }
      viewModel.Body = body;
    }
  }
}