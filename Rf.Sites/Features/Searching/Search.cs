using FubuMVC.Core;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.SiteInfrastructure;
using System.Linq;

namespace Rf.Sites.Features.Searching
{
    [HasActions]
    public class Search
    {
        private readonly ISearchPlugin[] _searchPlugins;

        public Search(ISearchPlugin[] searchPlugins)
        {
            _searchPlugins = searchPlugins;
        }

        
        public IJsonResponse Lookup(SearchTerm search)
        {
            return new SearchTextResponse(_searchPlugins.SelectMany(sp => sp.LinksFor(search.term)).Take(15).ToList());
        }
    }
}