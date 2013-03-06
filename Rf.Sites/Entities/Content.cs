using System;
using System.Collections.Generic;
using Rf.Sites.Frame;

namespace Rf.Sites.Entities
{
    public class Content : Entity
    {
        public Content() { }

        public Content(int id)
        {
            Id = id;
        }
        
        public virtual string Title { get; set; }
        
        public virtual string Body { get; set; }
        
        public virtual string Teaser { get; protected set; }

        public virtual string MetaKeyWords { get; set; }

        public virtual bool Published { get; set; }

        public virtual int AttachmentCount { get; protected set; }

        public virtual bool? IsMarkdown { get; set; }

        public virtual IList<Tag> Tags { get; protected set; }

        public virtual IList<Attachment> Attachments { get; protected set; }

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
            Teaser = new TagRemover(200).Process(Body);
        }

        public virtual void AddAttachment(Attachment attachment)
        {
            (Attachments ?? (Attachments = new List<Attachment>())).Add(attachment);
            AttachmentCount++;
        }
    }
}
