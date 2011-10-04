using System;

namespace Rf.Sites.Frame
{
    public class SiteSettings
    {
        public string SiteAuthor { get; set; }
        public string SiteTitle { get; set; }
        public string SiteCopyright { get; set; }
        public string DisqusSiteIdentifier { get; set; }
        public string DisqusDeveloperMode { get; set; }

        public string AdminUser { get; set; }
        public string AdminPassword { get; set; }
    }
}