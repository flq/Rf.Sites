using MarkdownSharp;
using Rf.Sites.Entities;

namespace Rf.Sites.Features.Models
{
    public class MarkdownFilter
    {
        private readonly Content _content;

        public MarkdownFilter(Content content)
        {
            _content = content;
        }

        private Content GetContent()
        {
            if (_content != null && _content.IsMarkdown != null && _content.IsMarkdown.Value)
            {
                var md = new Markdown(new MarkdownOptions() { AutoHyperlink = true});
                // Code is converted to <pre><code>, I just want the code tag:
                var html = md.Transform(_content.Body).Replace("<pre><code>", "<code>").Replace("</code></pre>", "</code>");
                _content.SetBody(html);
                return _content;
            }
            return _content;
        }

        public static implicit operator Content(MarkdownFilter filter)
        {
            return filter.GetContent();
        }
    }
}