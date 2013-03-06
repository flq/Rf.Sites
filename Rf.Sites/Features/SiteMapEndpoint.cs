using System;
using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features
{
    public class SiteMapEndpoint
    {
        private readonly IRepository<Content> _contents;
        private readonly string _urlTemplate;

        public SiteMapEndpoint(IRepository<Content> contents, IUrlRegistry urlRegistry, ServerVariables vars)
        {
            _contents = contents;
            _urlTemplate = urlRegistry.BuildAbsoluteUrlTemplate(vars, r => r.TemplateFor(new ContentId()));
        }

        [UrlPattern("sitemap.site")]
        public SiteMapModel GetSitemap()
        {
            return new SiteMapModel(_urlTemplate, _contents.Where(c => c.Created < DateTime.Now.ToUniversalTime()).OrderByDescending(c => c.Created));
        }
    }
}