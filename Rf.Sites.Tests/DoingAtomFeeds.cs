using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Tests.Frame;
using System.Linq;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class DoingAtomFeeds
  {
    private ContentFragments fragments;

    private static readonly Environment env =
      new Environment
        {
          AbsoluteBaseUrl = new Uri("http://host/Content/Entry/"),
          AuthorName = "Frank",
          CopyrightNotice = "All content hosted by this site is written by F Quednau. Reproduction only under consent"
        };
    
    [TestFixtureSetUp]
    public void Given()
    {
      var items =
        new List<ContentFragmentViewModel>
          {
            new ContentFragmentViewModel(1, "Hello", new DateTime(2008, 1, 1), "This is the body"),
            new ContentFragmentViewModel(2, "World", new DateTime(2007, 1, 20), "This is the body"),
            new ContentFragmentViewModel(3, "How", new DateTime(2009, 1, 1), "This is the body")
          };

      fragments = new ContentFragments(items) { Title = "The RSS Feed" };
    }

    [Test]
    public void ContentToAtomFeedworksWithContent()
    {
      var f = new ContentToFeedResponse(fragments, env);
      f.ContentType.ShouldBeEqualTo("application/rss+xml");
      var output = f.GetResponseWriterOutput();
      output.Length.ShouldBeGreaterThan(0);
      Console.WriteLine(output);

      var feed = output.GetAsSyndicationFeed();
      feed.Items.Count().ShouldBeEqualTo(3);
    }

    [Test]
    public void ItemTimeIsReflected()
    {
      var f = new ContentToFeedResponse(fragments, env);
      var feed = f.GetResponseWriterOutput().GetAsSyndicationFeed();

      var items = feed.Items.ToArray();
      items[0].LastUpdatedTime.ShouldBeEqualTo(new DateTimeOffset(new DateTime(2008, 1, 1)));
    }

    [Test]
    public void FeedLastUpdateIsNewestItem()
    {
      var f = new ContentToFeedResponse(fragments, env);
      var feed = f.GetResponseWriterOutput().GetAsSyndicationFeed();

      feed.LastUpdatedTime.ShouldBeEqualTo(new DateTimeOffset(new DateTime(2009, 1, 1)));
    }
  }
}