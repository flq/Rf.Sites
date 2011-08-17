namespace Rf.Sites.Features.DisplayContent
{
    public class ContentId
    {
        public int Id { get; set; }
        public static implicit operator int(ContentId cId)
        {
            return cId.Id;
        }
    }
}