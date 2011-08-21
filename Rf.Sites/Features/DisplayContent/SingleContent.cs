using System;
using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using Rf.Sites.Entities;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.DisplayContent
{
    [HasActions]
    public class SingleContent
    {
        private readonly IRepository<Content> _repository;

        public SingleContent(IRepository<Content> repository)
        {
            _repository = repository;
        }

        [UrlPattern("go/{Id}")]
        public ContentContinuation<Content, ContentVM> GetContent(ContentId contentId)
        {
            return 
                new ContentContinuation<Content, ContentVM>(_repository[contentId])
                  .ConditionalTransfer(
                    model => model == null, 
                    model => new InputModel404("Content with id " + contentId + " does not exist"))
                  .ConditionalTransfer(
                    model => model.Created > DateTime.UtcNow, 
                    model => new NotYetPublishedVM(model));
        }

        [UrlPattern("Content/Entry/{Id}")]
        public ContentContinuation<Content, ContentVM> GetAlternateContent(ContentId contentId)
        {
            return GetContent(contentId);
        }
    }
}