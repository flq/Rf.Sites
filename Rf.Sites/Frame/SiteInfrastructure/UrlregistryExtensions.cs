using System;
using FubuMVC.Core.Urls;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public static class UrlregistryExtensions
    {
        public static string BuildAbsoluteUrlTemplate(this IUrlRegistry registry, ServerVariables vars, Func<IUrlRegistry,string> templateselector)
        {
            return string.Format("http://{0}:{1}{2}", vars[ServerVariables.SERVER_NAME], vars[ServerVariables.SERVER_PORT], templateselector(registry));
        }
    }
}