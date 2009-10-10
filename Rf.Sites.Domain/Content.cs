using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using Rf.Sites.Domain.Frame;

namespace Rf.Sites.Domain
{
  public class Content : Entity
  {
    public Content() { }

    public Content(int id)
    {
      Id = id;
    }

    [NotNullNotEmpty, Length(255)]
    public virtual string Title { get; set; }
    
    [NotNullNotEmpty]
    public virtual string Body { get; set; }

    [Length(200)]
    public virtual string Teaser { get; private set; }

    public virtual string MetaKeyWords { get; set; }

    [NotNull]
    public virtual bool Published { get; set; }

    public virtual int CommentCount { get; private set; }

    public virtual IList<Comment> Comments { get; private set; }

    public virtual IList<Tag> Tags { get; private set; }

    public virtual void AssociateWithTag(Tag tag)
    {
      (Tags ?? (Tags = new List<Tag>())).Add(tag);
      tag.RelatedContent.Add(this);
    }

    public virtual void SetBody(string body)
    {
      if (body == null)
        throw new ArgumentNullException("body");
      Body = body;
      Teaser = new TeaserGenerator(200).Process(Body);
    }

    public virtual void AddComment(Comment comment)
    {
      (Comments ?? (Comments = new List<Comment>())).Add(comment);
      CommentCount++;
    }
  }
}
