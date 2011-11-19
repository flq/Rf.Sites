using System.Collections.Generic;

namespace Rf.Sites.Entities
{
  public class Tag : Entity
  {
    private IList<Content> relatedContent;

    public virtual string Name { get; set; }
    public virtual string Description { get; set; }

    public virtual IList<Content> RelatedContent
    {
      get { return relatedContent ?? (relatedContent = new List<Content>()); }
      private set { relatedContent = value; }
    }
  }
}