using FubuMVC.Core;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features
{
    [HasActions]
    public class Search
    {
        private readonly IRepository<Content> _repository;

        public Search(IRepository<Content> repository)
        {
            _repository = repository;
        }

        [UrlPattern("lookup")]
        public SearchTextResponse Lookup(SearchTerm search)
        {
            return new SearchTextResponse();
        }


    }
}