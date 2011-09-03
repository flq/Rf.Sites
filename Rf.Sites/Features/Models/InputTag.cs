using FubuMVC.Core;

namespace Rf.Sites.Features.Models
{
    public class InputTag : IPagingArgs
    {
        public InputTag() { }

        public InputTag(string tag)
        {
            Tag = tag;
        }

        [RouteInput]
        public string Tag { get; set; }

        [RouteInput]
        public int Page { get; set; }

        public string TotalCountCacheKey
        {
            get { return Tag; }
        }

        public string Title
        {
            get { return "Content tagged " + Tag; }
        }
    }
}