using System;
using FubuMVC.Core;

namespace Rf.Sites.Features.Models
{
    public class PagingArgs
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
}