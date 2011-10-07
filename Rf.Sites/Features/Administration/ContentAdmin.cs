using FubuMVC.Core;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Administration
{
    [HasActions]
    public class ContentAdmin
    {
        private readonly IContentAdministration _contentAdministration;

        public ContentAdmin(IContentAdministration contentAdministration)
        {
            _contentAdministration = contentAdministration;
        }

        [UrlRegistryCategory("Admin")]
        public object Get(ContentId contentId)
        {
            return new { contentId.Id, Cool = "Text" };
        }

        [UrlRegistryCategory("Admin")]
        public object Tags()
        {
            return _contentAdministration.GetTags();
        }

        [UrlRegistryCategory("Admin")]
        public void Post(dynamic content)
        {
            var stuff = content.test;
        }

        [UrlRegistryCategory("Admin")]
        public void Put(dynamic content)
        {
        }
    }
}