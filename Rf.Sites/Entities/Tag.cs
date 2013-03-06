using System.Collections.Generic;

namespace Rf.Sites.Entities
{
  public class Tag : Entity
  {
    private IList<Content> _relatedContent;

    public virtual string Name { get; set; }
    public virtual string Description { get; set; }

    public virtual IList<Content> RelatedContent
    {
      get { return _relatedContent ?? (_relatedContent = new List<Content>()); }
      protected set { _relatedContent = value; }
    }
  }
}