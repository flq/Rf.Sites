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

        public SearchTextResponse()
        {

        }

        public IEnumerator<SearchResult> GetEnumerator()
        {
            yield return new SearchResult { id = "A", value = "A test"};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class SearchResult
    {
        public string id { get; set; }
        public string value { get; set; }
    }
}