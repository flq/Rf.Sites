using MarkdownSharp;

namespace Rf.Sites.Features.Models
{
    public class MarkdownTransformer
    {
        private readonly string _content;

        public MarkdownTransformer(string content)
        {
            _content = content;
        }

        private string GetContent()
        {
            var md = new Markdown(new MarkdownOptions { AutoHyperlink = true });
            // Code is converted to <pre><code>, I just want the code tag:
            var html = md.Transform(_content).Replace("<pre><code>", "<code>").Replace("</code></pre>", "</code>");
            return html;
        }

        public static implicit operator string(MarkdownTransformer transformer)
        {
            return transformer.GetContent();
        }
    }
}