using FubuMVC.Core;

namespace Rf.Sites.Features.Models
{
    public class ContentId
    {

        public ContentId() { }

        public ContentId(int id)
        {
            Id = id;
        }

        [RouteInput]
        public int Id { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}