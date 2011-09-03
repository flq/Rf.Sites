using System;
using FubuMVC.Core;

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
        [RouteInput]
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
}