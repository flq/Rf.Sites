using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using Rf.Sites.Actions;
using Rf.Sites.Models;
using System.Linq;

namespace Rf.Sites.Frame
{
  public class ContentToFeedResponse : IResponseWriter
  {
    private readonly SyndicationFeed feed;

    public ContentToFeedResponse(ContentFragments fragments, Environment env)
    {
      var urlToContentItem = FrameUtilities.RelativeUrlToAction<ContentEntryAction>() + "/";

      var items = fragments
        .Select(vm => 
          new SyndicationItem(vm.Title, vm.Teaser, new Uri(env.ApplicationBaseUrl, urlToContentItem + vm.Id.ToString()))
            {
              Id = vm.Id.ToString(),
              LastUpdatedTime = new DateTimeOffset(vm.Created)
            });

      var lastUpdate = fragments.Max(vm => vm.Created);
      var title = env.SiteTitle + " | " + fragments.Title;
      var feedId = title.Replace(" ", "").GetHashCode().ToString();


      feed = new SyndicationFeed(items)
               {
                 Id = feedId,
                 Title = new TextSyndicationContent(title, TextSyndicationContentKind.Plaintext),
                 LastUpdatedTime = new DateTimeOffset(lastUpdate),
               };
      feed.Authors.Add(new SyndicationPerson(null, env.SiteMasterName, null));
      feed.Copyright = new TextSyndicationContent(env.CopyrightNotice);
    }

    public string ContentType
    {
      get { return "application/rss+xml"; }
    }

    public void WriteTo(TextWriter writer)
    {
      var s = new XmlWriterSettings { Indent = true };
      using (var xw = XmlWriter.Create(writer, s))
        feed.GetAtom10Formatter().WriteTo(xw);
    }
  }
}