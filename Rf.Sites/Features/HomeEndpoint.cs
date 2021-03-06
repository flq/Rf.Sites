using System;
using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features
{
    public class HomeEndpoint
    {
        private readonly IRepository<Content> _contentRepository;
        private object _inputModel;

        public HomeEndpoint(IRepository<Content> contentRepository, SubdomainSettings settings, ServerVariables vars)
        {
            _inputModel = settings.GetHomeInputModel(vars[ServerVariables.SERVER_NAME]);
            _contentRepository = contentRepository;
        }

        public FubuContinuation Index()
        {
            if (_inputModel == null)
              return FubuContinuation.TransferTo<HomeEndpoint>(h => h.All(new PagingArgs { Page = 0 }));
            return FubuContinuation.TransferTo(_inputModel);
        }

        [FubuPartial]
        public PersonalEntryPage PersonalEntry(PersonalEntryModel model)
        {
            return new PersonalEntryPage();
        }

        public ContentTeaserPage All(PagingArgs paging)
        {
            var query = GetQuery();
            var page = new ContentTeaserPage(paging, query);
            return page;
        }

        private IQueryable<ContentTeaserVM> GetQuery()
        {
            return from c in _contentRepository
                   where c.Created < DateTime.Now.ToUniversalTime()
                   orderby c.Created descending
                   select new ContentTeaserVM(c.Id, c.Title, c.Created, c.Teaser);
        }
    }
}