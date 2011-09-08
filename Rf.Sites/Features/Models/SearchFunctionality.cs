using System.Collections;
using System.Collections.Generic;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Models
{
    public class SearchTerm
    {
        public string term { get; set; }
    }

    public class SearchTextResponse : IJsonResponse, IEnumerable<SearchResult>
    {
        private readonly IEnumerable<SearchResult> _results;

        public SearchTextResponse(IEnumerable<SearchResult> results)
        {
            _results = results;
        }

        public IEnumerator<SearchResult> GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class SearchResult
    {
        public string prefixtext { get; set; }
        public Link[] values { get; set; }
    }

    public class Link
    {
        public string link;
        public string linktext;
    }
}