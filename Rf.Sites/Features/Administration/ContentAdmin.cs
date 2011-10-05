using System;
using System.IO;
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
        private readonly IRepository<Entities.Content> _content;

        public ContentAdmin(IFubuRequest req, IRepository<Entities.Content> content)
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
        public void Post(Content content)
        {
            var access = content.DynamicAccessToJson();
            var stuff = access.test;
        }

        [UrlRegistryCategory("Admin")]
        public void Put(Content content)
        {
        }
    }
}