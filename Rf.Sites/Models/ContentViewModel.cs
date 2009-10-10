using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using System.Linq;

namespace Rf.Sites.Models
{
  public class ContentViewModel
  {
    private readonly Content content;

    public ContentViewModel(Content content)
    {
      this.content = content;
    }

    public string Title
    {
      get { return content.Title; }
    }

    public string WrittenInTime
    {
      get { return content.Created.ToString("dd.MM.yyyy - hh:mm"); }
    }

    public string WrittenInPeriod
    {
      get
      {
        var o = new PeriodOfTimeOutput(content.Created, DateTime.Now).ToString();
        return string.Format("{0}{1}", o, o == "today" ? "" : " ago");
      }
    }

    public string Body
    {
      get { return content.Body; }
    }

    public IEnumerable<string> Tags
    {
      get
      {
        return content.Tags.Select(t => t.Name);
      }
    }
  }
}