using System;
using System.Collections.Generic;

namespace Rf.Sites.Features.Models
{
    public enum FeedType
    {
        Rss,
        Atom
    }

    public class FeedSetup
    {
        public IEnumerable<ContentTeaserVM> Content { get; set; }
        public string UrlTemplate { get; set; }
        public string Title { get; set; }
        public string SiteMasterName { get; set; }
        public string CopyrightNotice { get; set; }
        public FeedType FeedType { get; set; }

        public string FeedId
        {
            get { return Title.Replace(" ", "").GetHashCode().ToString(); }
        }

        public void AppendSubtitle(string subTitle)
        {
            Title = Title + " | " + subTitle;
        }
    }
}