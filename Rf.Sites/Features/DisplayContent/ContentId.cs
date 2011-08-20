using FubuMVC.Core;

namespace Rf.Sites.Features.DisplayContent
{
    public class ContentId
    {
        [RouteInput]
        public int Id { get; set; }

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