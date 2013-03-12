using System;
using FubuMVC.Core;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Models
{
    public interface IPagingArgs
    {
        int Page { get; set; }

        string TotalCountCacheKey { get; }
        string Title { get; }
    }

    public class PagingArgs : IPagingArgs
    {
        [RouteInput("0")]
        public int Page { get; set; }

        public virtual string TotalCountCacheKey
        {
            get { return "all"; }
        }

        public virtual string Title
        {
            get { return "All Content"; }
        }
    }

    public class YearPaging : PagingArgs
    {
        [RouteInput]
        public int Year { get; set; }

        public override string Title
        {
            get { return "Content from year " + Year; }
        }

        public override string TotalCountCacheKey
        {
            get { return "content" + Year; }
        }
    }

    [RouteParameterSorter("Year", "Month", "Page")]
    public class MonthPaging : YearPaging
    {
        [RouteInput]
        public int Month { get; set; }

        public override string Title
        {
            get { return "Content from month " + Year + "/" + Month; }
        }

        public override string TotalCountCacheKey
        {
            get { return "content" + Year + Month; }
        }
    }

    public class TagPaging : PagingArgs
    {
        public TagPaging() { }

        public TagPaging(string tag) { Tag = tag; }

        [RouteInput]
        public string Tag { get; set; }

        public override string TotalCountCacheKey { get { return "tag" + Tag; } }

        public override string Title { get { return "Content tagged " + Tag; } }
    }
}