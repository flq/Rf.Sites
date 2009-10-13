using System;
using System.Collections.Generic;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using System.Linq;

namespace Rf.Sites.Models
{
  public class ContentViewModel
  {
    public ContentViewModel(Content content, IVmExtender<ContentViewModel>[] extender)
    {
      pullData(content);
      if (extender != null)
        callExtender(extender);
    }

    public string Title { get; private set; }

    public string Keywords { get; private set; }

    public string WrittenInTime { get; private set; }

    public string WrittenInPeriod { get; private set; }

    public string Body { get; set; }

    public bool NeedsCodeHighlighting { get; set; }

    public IEnumerable<string> Tags { get; private set; }

    private void callExtender(IEnumerable<IVmExtender<ContentViewModel>> extenders)
    {
      foreach (var e in extenders)
        e.Inspect(this);
    }

    private void pullData(Content content)
    {
      Title = content.Title;
      WrittenInTime = content.Created.ToString("dd.MM.yyyy - hh:mm");
      var o = new PeriodOfTimeOutput(content.Created, DateTime.Now).ToString();
      WrittenInPeriod = string.Format("{0}{1}", o, o == "today" ? "" : " ago");
      Body = content.Body;
      Keywords = content.MetaKeyWords;
      Tags = content.Tags != null ? content.Tags.Select(t => t.Name) : new string[] {};
    }
  }
}