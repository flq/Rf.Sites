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

        public static implicit operator int(ContentId cId)
        {
            return cId.Id;
        }

        public static implicit operator ContentId(int id)
        {
            return new ContentId { Id = id };
        }
    }
}