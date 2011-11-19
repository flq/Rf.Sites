using System;

namespace Rf.Sites.Entities
{
  public class Entity
  {
    public virtual int Id { get; protected set; }
    public virtual DateTime Created { get; set; }
    
  }
}