namespace Rf.Sites.Entities
{
  public class Attachment : Entity
  {
    public virtual string Name { get; set; }
    public virtual string Path { get; set; }
    public virtual int Size { get; set; }

  }
}