using NHibernate.Validator.Constraints;

namespace Rf.Sites.Domain
{
  public class Comment : Entity
  {
    [NotNullNotEmpty]
    public virtual string CommenterName { get; set; }
    public virtual string CommenterEmail { get; set; }
    public virtual string CommenterWebsite { get; set; }
    [NotNullNotEmpty, Length(2000)]
    public virtual string Body { get; set; }

  }
}