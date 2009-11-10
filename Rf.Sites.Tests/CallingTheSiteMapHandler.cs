using System.Xml;
using Moq;
using NHibernate;
using NUnit.Framework;
using Rf.Sites.Handlers;
using Rf.Sites.Tests.Frame;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class CallingTheSiteMapHandler
  {
    private HandlerEnv env;
    private TestHttpContext usedContext;
    private XmlDocument output;

    [TestFixtureSetUp]
    public void Setup()
    {
      env = new HandlerEnv();
      env.UseInMemoryDb();

      var session = env.InMemoryDB.Session;
      using (var t = session.BeginTransaction())
      {
        var cnt = env.InMemoryDB.Maker.CreateContent();
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
    }

    [Test]
    public void TheOutputIsAValidDocument()
    {
      output.ShouldNotBeNull();
    }
  }
}