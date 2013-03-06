using FubuMVC.Core;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.SiteInfrastructure;
using System.Linq;

namespace Rf.Sites.Features.Searching
{
    public class SearchEndpoint
    {
        private readonly ISearchPlugin[] _searchPlugins;

        public SearchEndpoint(ISearchPlugin[] searchPlugins)
        {
            _searchPlugins = searchPlugins;
        }


        public SearchTextResponse Lookup(SearchTerm search)
        {
            return new SearchTextResponse(_searchPlugins.SelectMany(sp => sp.LinksFor(search.term)).Take(15).ToList());
        }
    }
}