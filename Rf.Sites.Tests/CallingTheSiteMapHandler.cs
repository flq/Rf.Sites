using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Moq;
using NHibernate;
using NUnit.Framework;
using Rf.Sites.Domain;
using Rf.Sites.Handlers;
using Rf.Sites.Tests.Frame;
using Environment=Rf.Sites.Frame.Environment;
using System.Linq;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class CallingTheSiteMapHandler
  {
    private HandlerEnv env;
    private TestHttpContext usedContext;
    private XmlDocument output;
    private List<Content> content;
    private XmlNamespaceManager nsMgr;

    [TestFixtureSetUp]
    public void Setup()
    {
      env = new HandlerEnv();
      env.UseInMemoryDb();

      var session = env.InMemoryDB.Session;
      using (var t = session.BeginTransaction())
      {
        content = createContent(env.InMemoryDB.Maker);
        foreach (var cnt in content)
          session.Save(cnt);
        t.Commit();
      }
      session.Clear();

      var sF = new Mock<ISessionFactory>();
      sF.Setup(f => f.OpenStatelessSession())
        .Returns(env.InMemoryDB.Factory.OpenStatelessSession(session.Connection));

      var siteEnv = env.Container.GetInstance<Environment>();

      env.ExecuteHandler(new SiteMapHandler(siteEnv, sF.Object));
      usedContext = env.UsedContext;
      output = new XmlDocument();
      output.LoadXml(usedContext.OutputOfHandler.ToString());

      nsMgr = new XmlNamespaceManager(output.NameTable);
      nsMgr.AddNamespace("ns0", "http://www.sitemaps.org/schemas/sitemap/0.9");
    }

    [Test]
    public void TheOutputIsAValidDocument()
    {
      output.ShouldNotBeNull();
    }

    [Test]
    public void ChangeFrequencyOfRecentContentIsCorrect()
    {
      var node = getTheNodeOf(c => c.Title == "A");
      node.SelectSingleNode("ns0:changefreq", nsMgr).InnerText.ShouldBeEqualTo("weekly");
    }

    [Test]
    public void ChangeFrequencyOfOldContentIsCorrect()
    {
      var node = getTheNodeOf(c => c.Title == "C");
      node.SelectSingleNode("ns0:changefreq", nsMgr).InnerText.ShouldBeEqualTo("never");
    }

    [Test]
    public void TheLatestDateWasPickedBetweenCommentAndContent()
    {
      var node = getTheNodeOf(c => c.Title == "C");
      var time = DateTime.ParseExact(node.SelectSingleNode("ns0:lastmod", nsMgr).InnerText, "yyyy-MM-dd",
                                     DateTimeFormatInfo.InvariantInfo);
      time.ShouldBeEqualTo(new DateTime(2006,6,6));
    }

    [Test]
    public void NumberOfCommentsInfluencesContentPriority()
    {
      var node = getTheNodeOf(c => c.Title == "B");
      node.SelectSingleNode("ns0:priority", nsMgr).InnerText.ShouldBeEqualTo("0.7");
    }

    private static List<Content> createContent(EntityMaker maker)
    {
      List<Content> contents = new List<Content>();
      
      var cnt = maker.CreateContent("A");
      cnt.Created = DateTime.Now.Subtract(TimeSpan.FromDays(1));
      contents.Add(cnt);
      
      cnt = maker.CreateContent("B");
      cnt.AddComment(maker.CreateComment());
      cnt.AddComment(maker.CreateComment());
      cnt.AddComment(maker.CreateComment());
      cnt.Created = DateTime.Now.Subtract(TimeSpan.FromDays(60));
      contents.Add(cnt);
      
      cnt = maker.CreateContent("C");
      cnt.Created = new DateTime(2005,5,5);
      var comment = maker.CreateComment();
      comment.Created = new DateTime(2006,6,6);
      cnt.AddComment(comment);
      contents.Add(cnt);

      return contents;
    }

    private XmlNode getTheNodeOf(Func<Content, bool> func)
    {
      var cnt = content.Where(func).Single();
      var xpath = "//ns0:url[contains(./ns0:loc,'" + cnt.Id + "')]";
      var node = output.SelectSingleNode(xpath, nsMgr);
      return node;
    }
  }
}