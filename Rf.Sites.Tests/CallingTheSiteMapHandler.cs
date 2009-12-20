using System;
using System.Globalization;
using System.Xml;
using NUnit.Framework;
using Rf.Sites.Domain;
using Rf.Sites.Handlers;
using Rf.Sites.Tests.DataScenarios;
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
    private ContentWithComments scenario;
    private XmlNamespaceManager nsMgr;

    [TestFixtureSetUp]
    public void Setup()
    {
      env = new HandlerEnv();
      env.UseInMemoryDb();
      scenario = env.DataScenario<ContentWithComments>();

      var siteEnv = new Environment() { ApplicationBaseUrl = new Uri("http://localhost")};

      env.ExecuteHandler(new SiteMapHandler(siteEnv, env.FactoryForStatelessSession()));
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

    [Test]
    public void UrlIsSetupCorrectly()
    {
      const string correctUrl = "http://localhost/Content/Entry/1";
      var node = getTheNodeOf(c => c.Title == "A");
      node.SelectSingleNode("ns0:loc", nsMgr).InnerText.ShouldBeEqualTo(correctUrl);
    }

    private XmlNode getTheNodeOf(Func<Content, bool> func)
    {
      var cnt = scenario.Contents.Where(func).Single();
      var xpath = "//ns0:url[contains(./ns0:loc,'" + cnt.Id + "')]";
      var node = output.SelectSingleNode(xpath, nsMgr);
      return node;
    }
  }
}