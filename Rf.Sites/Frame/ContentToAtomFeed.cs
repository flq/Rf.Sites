using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using Rf.Sites.Models;
using System.Linq;

namespace Rf.Sites.Frame
{
  public class ContentToAtomFeed : IResponseWriter
  {
    private readonly SyndicationFeed feed;

    public ContentToAtomFeed(ContentFragments fragments, Environment env)
    {
      var items = fragments
        .Select(vm => 
          new SyndicationItem(vm.Title, vm.Teaser, new Uri(env.AbsoluteBaseUrl, vm.Id.ToString()))
            {
              LastUpdatedTime = new DateTimeOffset(vm.Created)
            });

      var lastUpdate = fragments.Max(vm => vm.Created);

      feed = new SyndicationFeed(items)
               {
                 Title = new TextSyndicationContent(fragments.Title, TextSyndicationContentKind.Plaintext),
                 LastUpdatedTime = new DateTimeOffset(lastUpdate)
               };
      feed.Authors.Add(new SyndicationPerson(null, env.AuthorName, null));
      feed.Copyright = new TextSyndicationContent(env.CopyrightNotice);
    }

    public string ContentType
    {
      get { return "application/rss+xml"; }
    }

    public void WriteTo(Stream stream)
    {
      var s = new XmlWriterSettings { Indent = true };
      using (var xw = XmlWriter.Create(stream, s))
        feed.GetAtom10Formatter().WriteTo(xw);
    }
  }
}