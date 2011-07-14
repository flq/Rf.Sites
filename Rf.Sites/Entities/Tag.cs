using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace Rf.Sites.Entities
{
  public class Tag : Entity
  {
    private IList<Content> relatedContent;

    [NotNullNotEmpty, Length(255)]
    public virtual string Name { get; set; }

    [Length(255)]
    public virtual string Description { get; set; }

    public virtual IList<Content> RelatedContent
    {
      get { return relatedContent ?? (relatedContent = new List<Content>()); }
      private set { relatedContent = value; }
    }
  }
}