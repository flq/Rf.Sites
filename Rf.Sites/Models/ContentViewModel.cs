using System;
using System.Collections.Generic;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using System.Linq;
using StructureMap;

namespace Rf.Sites.Models
{
  public class ContentViewModel
  {
    
    public ContentViewModel(Content content, IVmExtender<ContentViewModel>[] extender)
    {
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
      WrittenInTime = content.Created.ToString("dd.MM.yyyy - hh:mm");
      var o = new PeriodOfTimeOutput(content.Created, DateTime.Now).ToString();
      WrittenInPeriod = string.Format("{0}{1}", o, o == "today" ? "" : " ago");
      Body = content.Body;
      Keywords = content.MetaKeyWords;
      Tags = content.Tags != null ? content.Tags.Select(t => t.Name) : new string[] {};
      
      CommentCount = content.CommentCount;

    }
  }
}