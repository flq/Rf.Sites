using FubuMVC.Core;

namespace Rf.Sites.Features.Administration
{
    public class ContentId
    {
        public ContentId() { }

        public ContentId(string id)
        {
            Id = id;
        }

        [RouteInput]
        public string Id { get; set; }

        public override string ToString() { return Id; }
    }
}