using System;
using System.Collections.Generic;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using System.Linq;

namespace Rf.Sites.Models
{
  public class ContentViewModel
  {
    private readonly IObjectConverter<Comment, CommentVM> converter;

    public ContentViewModel(Content content, IVmExtender<ContentViewModel>[] extender)
      : this(content,extender,ObjectConverter.From((Comment c)=>new CommentVM(c,null)))
    {}

    public ContentViewModel(
      Content content, 
      IVmExtender<ContentViewModel>[] extender,
      IObjectConverter<Comment,CommentVM> converter)
    {
      this.converter = converter;
      pullData(content);
      extender.Apply(this);
    }

    public string Title { get; private set; }

    public string Keywords { get; private set; }

    public string WrittenInTime { get; private set; }

    public string WrittenInPeriod { get; private set; }

    public string Body { get; set; }

    public bool NeedsCodeHighlighting { get; set; }

    public int CommentCount { get; set; }

    public IEnumerable<string> Tags { get; private set; }

    public IEnumerable<CommentVM> Comments { get; private set; }

    private void pullData(Content content)
    {
      Title = content.Title;
      WrittenInTime = content.Created.ToString(Constants.CommonDateFormat);
      var o = new PeriodOfTimeOutput(content.Created, DateTime.Now).ToString();
      WrittenInPeriod = string.Format("{0}{1}", o, o == "today" ? "" : " ago");
      Body = content.Body;
      Keywords = content.MetaKeyWords;
      Tags = content.Tags != null ? content.Tags.Select(t => t.Name) : new string[] {};
      
      CommentCount = content.CommentCount;
      if (CommentCount == 0) return;

      Comments = from cmt in content.Comments
                 select converter.Convert(cmt);

    }
  }
}