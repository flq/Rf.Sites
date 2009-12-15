using System;
using System.Collections.Generic;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using System.Linq;

namespace Rf.Sites.Models
{
  public class ContentViewModel
  {
    private readonly IObjectConverter<Comment, CommentVM> commentConverter;
    private readonly IObjectConverter<Attachment, AttachmentVM> attachmentConverter;

    public ContentViewModel(Content content, IVmExtender<ContentViewModel>[] extender)
      : this(content,extender,
      ObjectConverter.From((Comment c)=>new CommentVM(c,null)),
      ObjectConverter.From((Attachment a) => new AttachmentVM(a,null)))
    {}

    public ContentViewModel(
      Content content, 
      IVmExtender<ContentViewModel>[] extender,
      IObjectConverter<Comment,CommentVM> commentConverter,
      IObjectConverter<Attachment, AttachmentVM> attachmentConverter)
    {
      this.commentConverter = commentConverter;
      this.attachmentConverter = attachmentConverter;
      pullData(content);
      extender.Apply(this);
    }

    public string ContentId { get; private set; }

    public string Title { get; private set; }

    public string Keywords { get; private set; }

    public string WrittenInTime { get; private set; }

    public string WrittenInPeriod { get; private set; }

    public string Body { get; set; }

    public bool NeedsCodeHighlighting { get; set; }

    public int AttachmentCount { get; private set; }

    public int CommentCount { get; set; }

    public IEnumerable<string> Tags { get; private set; }

    public IEnumerable<CommentVM> Comments { get; private set; }
    public IEnumerable<AttachmentVM> Attachments { get; private set; }

    private void pullData(Content content)
    {
      ContentId = content.Id.ToString();
      Title = content.Title;
      WrittenInTime = content.Created.ToString(Constants.CommonDateFormat);
      var o = new PeriodOfTimeOutput(content.Created, DateTime.Now).ToString();
      WrittenInPeriod = string.Format("{0}{1}", o, o == "today" ? "" : " ago");
      Body = content.Body;
      Keywords = content.MetaKeyWords;
      Tags = content.Tags != null ? content.Tags.Select(t => t.Name) : new string[] {};
      
      CommentCount = content.CommentCount;
      AttachmentCount = content.AttachmentCount;
      if (CommentCount > 0)
        Comments = from cmt in content.Comments select commentConverter.Convert(cmt);

      if (AttachmentCount > 0)
        Attachments = from a in content.Attachments select attachmentConverter.Convert(a);
    }
  }
}