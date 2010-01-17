using System;
using System.Xml.Serialization;

namespace Rf.Sites.Frame
{
  [XmlRoot]
  public class Environment
  {
    public Environment()
    {
      BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    }
    
    [XmlIgnore]
    public Uri ApplicationBaseUrl { get; set; }
    public string ApplicationBaseUrlAsString
    {
      get { return ApplicationBaseUrl.ToString(); }
      set { ApplicationBaseUrl = new Uri(value);}
    }

    public string CopyrightNotice { get; set; }

    public string SiteTitle { get; set; }

    public string SiteMasterName { get; set; }
    public string SiteMasterEmail { get; set; }
    public string SiteMasterPassword { get; set; }
    public string SiteMasterWebPage { get; set; }

    public string BaseDirectory { get; set; }
    public string DropZoneUrl { get; set; }

    public int FeedItemsPerFeed { get; set; }
    public int ItemsPerPage { get; set; }

    public int TagcloudSegments { get; set; }

    public bool CommentingEnabled { get; set; }
  }
}