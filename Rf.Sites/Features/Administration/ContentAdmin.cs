using FubuMVC.Core;
using FubuMVC.Core.Runtime;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;
using System.Linq;

namespace Rf.Sites.Features.Administration
{
    [HasActions]
    public class ContentAdmin
    {
        private readonly IRepository<Tag> _tags;

        public ContentAdmin(IRepository<Entities.Tag> tags)
        {
            _tags = tags;
        }

        [UrlRegistryCategory("Admin")]
        public IJsonResponse Get(ContentId contentId)
        {
            return null;
        }

        [UrlRegistryCategory("Admin")]
        public object GetTags()
        {
            return _tags.Select(t => t.Name).ToArray();
        }

        [UrlRegistryCategory("Admin")]
        public void Post(dynamic content)
        {
            var stuff = content.test;
        }

        [UrlRegistryCategory("Admin")]
        public void Put(Content content)
        {
        }
    }
}