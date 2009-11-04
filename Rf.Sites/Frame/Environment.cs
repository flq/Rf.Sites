using System;

namespace Rf.Sites.Frame
{
  public class Environment
  {
    public Uri AbsoluteBaseUrl { get; set; }
    public string CopyrightNotice { get; set; }

    public string SiteTitle { get; set; }

    public string SiteMasterName { get; set; }
    public string SiteMasterEmail { get; set; }
    public string SiteMasterPassword { get; set; }
    public string SiteMasterWebPage { get; set; }
    
    public string DropZoneUrl { get; set; }

    public int FeedItemsPerFeed { get; set; }
    public int ItemsPerPage { get; set; }
  }
}