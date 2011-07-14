using NHibernate.Validator.Constraints;

namespace Rf.Sites.Entities
{
  public class Attachment : Entity
  {
    [NotNullNotEmpty]
    public virtual string Name { get; set; }
    [NotNullNotEmpty]
    public virtual string Path { get; set; }
    public virtual int Size { get; set; }

  }
}