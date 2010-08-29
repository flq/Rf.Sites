using System;
using Rf.Sites.Frame;

namespace Rf.Sites.Models
{
  public class FutureContentViewModel
  {
    private readonly DateTime publishDate;

    public FutureContentViewModel(string title, DateTime publishDate)
    {
      this.publishDate = publishDate;
      Title = title;
    }

    public string Title { get; private set; }

    public string WrittenFuturePublishDate
    {
      get
      {
        var p = new PeriodOfTimeOutput(DateTime.Now, publishDate);
        return p.ToString();
      }
    }
  }
}