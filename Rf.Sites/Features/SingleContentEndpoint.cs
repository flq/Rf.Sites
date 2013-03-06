using FubuMVC.Core;
using FubuMVC.Core.Urls;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features
{
    public class SingleContentEndpoint
    {
        private readonly IRepository<Content> _repository;
        private readonly SiteSettings _settings;
        private readonly ServerVariables _vars;
        private readonly IUrlRegistry _reg;

        public SingleContentEndpoint(IRepository<Content> repository, SiteSettings settings, ServerVariables vars, IUrlRegistry reg)
        {
            _repository = repository;
            _settings = settings;
            _vars = vars;
            _reg = reg;
        }

        [UrlPattern("go/{Id}")]
        public ContentVM GetContent(ContentId contentId)
        {
            var c = _repository[contentId.Id];

            return c != null ? new ContentVM(c, _settings, _vars, _reg) : null;
            
        }
    }
}