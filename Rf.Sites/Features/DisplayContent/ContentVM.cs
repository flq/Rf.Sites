using System;
using System.Collections.Generic;
using System.Linq;
using Rf.Sites.Entities;
using Rf.Sites.Frame;

namespace Rf.Sites.Features.DisplayContent
{
  public class ContentVM
  {
    private readonly IObjectConverter<Attachment, AttachmentVM> _attachmentConverter;
    private bool commentingDisabled;

    public ContentVM(
      Content content, 
      IObjectConverter<Attachment, AttachmentVM> attachmentConverter)
    {
      this._attachmentConverter = attachmentConverter;
      pullData(content);
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

    public IEnumerable<AttachmentVM> Attachments { get; private set; }


    private void pullData(Content content)
    {
      ContentId = content.Id.ToString();
      Title = content.Title;
      WrittenInTime = content.Created.ToString(Constants.CommonDateFormat);
      var o = new PeriodOfTimeOutput(content.Created, DateTime.Now).ToString();
      WrittenInPeriod = string.Format("{0}{1}", o, o == "today" ? "" : " ago");
      commentingDisabled = content.CommentingDisabled;
      Body = content.Body;
      Keywords = content.MetaKeyWords;
      Tags = content.Tags != null ? content.Tags.Select(t => t.Name) : new string[] {};
      
      CommentCount = content.CommentCount;
      AttachmentCount = content.AttachmentCount;
      
      if (AttachmentCount > 0)
        Attachments = from a in content.Attachments select _attachmentConverter.Convert(a);
    }
  }
}