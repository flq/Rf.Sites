using FubuMVC.Core;
using FubuMVC.Core.Runtime;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Administration
{
    [HasActions]
    public class ContentAdmin
    {
        private readonly IFubuRequest _req;
        private readonly IRepository<Content> _content;

        public ContentAdmin(IFubuRequest req, IRepository<Content> content)
        {
            _req = req;
            _content = content;
        }

        [UrlRegistryCategory("Admin")]
        public IJsonResponse Get(ContentId contentId)
        {
            return null;
        }

        [UrlRegistryCategory("Admin")]
        public void Post(ContentAdministration content)
        {
            
        }

        [UrlRegistryCategory("Admin")]
        public void Put(ContentAdministration content)
        {
        }
    }

    public class ContentAdministration
    {
    }
}