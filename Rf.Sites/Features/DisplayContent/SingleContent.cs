using FubuMVC.Core;
using Rf.Sites.Entities;
using Rf.Sites.Frame;
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
        public ContentContinuation<Content,ContentVM> GetContent(ContentId contentId)
        {
            return new ContentContinuation<Content, ContentVM>(_repository[contentId]);
        }

        [UrlPattern("Content/Entry/{Id}")]
        public ContentContinuation<Content, ContentVM> GetAlternateContent(ContentId contentId)
        {
            return GetContent(contentId);
        }
    }
}