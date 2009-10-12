using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;
using NUnit.Framework;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class DoingAtomFeeds
  {
    [Test]
    public void SpikeUsingServiceModelAtomFormatter()
    {
      /// Idea is creating Actionresult that is for RssFeeds
      var items = new List<SyndicationItem>
                    {
                      new SyndicationItem("title", "content", new Uri("http://realfiction.net/a")),
                      new SyndicationItem("title2", "content2", new Uri("http://realfiction.net/b"))
                    };

      SyndicationFeed feed =
        new SyndicationFeed(
          "The blog", 
          "bla", 
          new Uri("http://realfiction.net/d"),
          items);
      feed.Authors.Add(new SyndicationPerson("", "Frank", "http://realfiction.net/c"));
      feed.Categories.Add(new SyndicationCategory("ho"));
      feed.LastUpdatedTime = DateTime.Now;

      var f = feed.GetAtom10Formatter();

      var s = new XmlWriterSettings {Indent = true};

      using (var xw = XmlWriter.Create(Console.Out, s))
      {
        f.WriteTo(xw);
      }

    }
  }
}