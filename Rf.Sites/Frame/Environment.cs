using System;

namespace Rf.Sites.Frame
{
  public class Environment
  {
    public Uri AbsoluteBaseUrl { get; set; }
    public string AuthorName { get; set; }
    public string CopyrightNotice { get; set; }

    public int FeedItemsPerFeed { get; set; }
    public int ItemsPerPage { get; set; }

  }
}