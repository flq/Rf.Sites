using System.Collections;
using System.Collections.Generic;

namespace Rf.Sites.Features.Models
{
    public class SearchTerm
    {
        public string term { get; set; }
    }

    public class SearchTextResponse : IEnumerable<Link>
    {
        private readonly IEnumerable<Link> _links;

        public SearchTextResponse(IEnumerable<Link> links)
        {
            _links = links;
        }

        public IEnumerator<Link> GetEnumerator()
        {
            return _links.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Link
    {
        public string link;
        public string linktext;
    }
}