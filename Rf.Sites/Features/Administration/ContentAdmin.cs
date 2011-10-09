using System;
using FubuMVC.Core;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Administration
{
    [HasActions]
    public class ContentAdmin
    {
        private readonly IContentAdministration _contentAdministration;
        private readonly IFubuRequest _request;
        private readonly IUrlRegistry _urls;
        private readonly ServerVariables _vars;

        public ContentAdmin(IContentAdministration contentAdministration, IFubuRequest request, IUrlRegistry urls, ServerVariables vars)
        {
            _contentAdministration = contentAdministration;
            _request = request;
            _urls = urls;
            _vars = vars;
        }

        [UrlRegistryCategory("Admin")]
        public object Get(ContentId contentId)
        {
            var @return = _contentAdministration.GetContent(contentId.Id);
            if (@return == null)
                throw new ContentAdminException("NotFound");
            return @return;
        }

        [UrlRegistryCategory("Admin")]
        public object Tags()
        {
            return _contentAdministration.GetTags();
        }

        [UrlRegistryCategory("Admin")]
        public void Post(dynamic content)
        {
            var newId = _contentAdministration.InsertContent(content);
            _request.Set(new InsertInfo(_urls.UrlFor(new ContentId(newId), "Admin")));
        }

        [UrlRegistryCategory("Admin")]
        public void Put(dynamic content)
        {
            var id = GetId();
            _contentAdministration.UpdateContent(id, content);
        }

        private string GetId()
        {
            var url = _vars[ServerVariables.URL];
            var sId = url.Substring(url.LastIndexOf("/") + 1);
            return sId;
        }
    }

    public class InsertInfo
    {
        public InsertInfo(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
    }
}