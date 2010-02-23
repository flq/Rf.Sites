using MarkdownSharp;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions.CommentPost
{
  [Order(3)]
  public class RunCommentThroughMarkdown : IVmExtender<CommentUpdatePreparer>
  {
    private static readonly Markdown md = new Markdown {AutoHyperlink = true};

    public void Inspect(CommentUpdatePreparer viewModel)
    {
      if (!viewModel.IsValid)
        return;
      var r = new TagRemover();
      var sAfterTagRemoval = r.Process(viewModel.Comment.Body);
      var afterMarkdownTransform = md.Transform(sAfterTagRemoval);

      viewModel.Comment.Body = afterMarkdownTransform;
    }

    public static string RunMarkdown(string text)
    {
      // Code is converted to <pre><code>, I just want the code tag:
      return md.Transform(text).Replace("<pre><code>", "<code>").Replace("</code></pre>", "</code>");
    }
  }
}