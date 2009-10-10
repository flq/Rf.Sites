using System;
using NHibernate.Validator.Constraints;

namespace Rf.Sites.Domain
{
  public class Entity
  {
    public virtual int Id { get; protected set; }
    [NotNull]
    public virtual DateTime Created { get; set; }
    
  }
}