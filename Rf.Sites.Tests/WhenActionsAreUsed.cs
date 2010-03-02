using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Moq;
using NUnit.Framework;
using Rf.Sites.Actions;
using Rf.Sites.Actions.Args;
using Rf.Sites.Actions.Comments;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Tests.DataScenarios;
using Rf.Sites.Tests.Frame;
using Moq.Protected;
using Environment=Rf.Sites.Frame.Environment;
using System.Linq;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class WhenActionsAreUsed : DbTestBase
  {
    readonly ActionEnv actionEnv = new ActionEnv();

    [TearDown]
    public void TearDown()
    {
      actionEnv.ReleaseResources();
    }

    [Test]
    public void ContentEntryReturnsLoadedContentInViewModel()
    {
      var mock = new Mock<IRepository<Content>>();
      mock.Setup(rep => rep[1]).Returns(new Content(1) {Title = "Foo", Body = "Bar"});
      
      var action = new ContentEntryAction(ArgsFrom.Id(1), mock.Object) {Container = actionEnv.Container};
      var model = action.Execute().GetModelFromAction<ContentViewModel>();
      
      model.Title.ShouldBeEqualTo("Foo");
      mock.Verify();
    }

    [Test]
    public void ContentEntryRedirectsToUnknownWhenIDCouldNotBeResolved()
    {
      var action = new ContentEntryAction(ArgsFrom.Id(-1), null) { Container = actionEnv.Container };
      var result = action.Execute();
      result.ShouldBeOfType<RedirectResult>();
      ((RedirectResult)result).Url.Contains("Unknown").ShouldBeTrue();
    }

    [Test]
    public void ContentEntryRedirectsToUnknownWhenIDDoesNotExist()
    {
      var mock = new Mock<IRepository<Content>>();
      mock.Setup(rep => rep[1]).Returns(new Content(1) { Title = "Foo", Body = "Bar" });
      var action = new ContentEntryAction(ArgsFrom.Id(3), mock.Object) { Container = actionEnv.Container };
      var result = action.Execute();
      result.ShouldBeOfType<RedirectResult>();
      ((RedirectResult)result).Url.Contains("Unknown").ShouldBeTrue();
    }

    [Test]
    public void CallByTagReturnsListViewModel()
    {
      var rep = new TestRepository<Content>();

      var c = new Content {Title = "Bar", Body = "Hello world!"};
      c.AssociateWithTag(new Tag { Name = "Foo" });
      rep.Add(c);
      var c2 = new Content { Title = "Baz", Body = "How are you?" };
      c2.AssociateWithTag(new Tag { Name = "Fluuoo" });
      rep.Add(c2);

      var action = new ContentTagAction(ArgsFrom.Value("Foo"), rep) { Environment = new Environment { ItemsPerPage = 5}};
      var model = action.Execute().GetModelFromAction<ContentFragments>();
      model.ShouldHaveCount(1);
      model.Title.ShouldBeEqualTo("Foo");
      model.CurrentPage.ShouldBeEqualTo(0);
      model[0].Title.ShouldBeEqualTo("Bar");
    }

    [Test]
    public void NHibernateCheckOfGettingTeasers()
    {
      actionEnv.UseInMemoryDb();
      var scenario = actionEnv.DataScenario<TagAndContent>();
      
      var action = new ContentTagAction(
        ArgsFrom.Value(scenario.Tag.Name), 
        new Repository<Content>(actionEnv.InMemoryDB.Factory))
        { Environment = new Environment { ItemsPerPage = 5}};

      var model = action.Execute().GetModelFromAction<List<ContentFragmentViewModel>>();
      model.ShouldHaveCount(1);

    }

    [Test]
    public void RssActionStructureWorksAsOutlined()
    {
      var finalResponse = new StringBuilder();
      actionEnv.ControllerCtxMock.ResponseMock.Setup(rB => rB.Output).Returns(new StringWriter(finalResponse));
      actionEnv.OverloadContainer(ce => ce.For<Environment>()
                                   .Use(new Environment
                                   {
                                     ApplicationBaseUrl = new Uri("http://localhost")
                                   }));
      var mock = new Mock<AbstractFeed>();
      mock.Protected().Setup<ContentFragments>("produceFragments")
        .Returns(
        () => new ContentFragments(new List<ContentFragmentViewModel>
                                     {
                                       new ContentFragmentViewModel(1, "Hello", DateTime.Now, "Blabla")
                                     })
                {
                  Title = "The Feed"
                });

      
      var action = actionEnv.GetAction(mock.Object);
      var result = action.Execute();
      result.ExecuteResult(actionEnv.ControllerCtxMock.ControllerContext);

      actionEnv.ControllerCtxMock.ResponseMock.VerifySet(rB => rB.ContentType);
      try
      {
        XmlDocument d = new XmlDocument();
        d.LoadXml(finalResponse.ToString());
      }
      catch
      {
        Assert.Fail("Output was not XML.");
      }
    }

    [Test]
    public void CommentListReturnsCorrectComments()
    {
      actionEnv.UseInMemoryDb();
      actionEnv.MockTheSessionFactoryForStateless();
      actionEnv
        .OverloadContainer(c=>c.For<Environment>()
          .Use(new Environment { ApplicationBaseUrl = new Uri("http://localhost")}));
      actionEnv.DataScenario<ContentWithComments2>();
      var a = actionEnv.GetAction<CommentsIndexAction>();
      var result = a.Execute();
      var list = result.GetModelFromAction<CommentList>();
      list.ShouldNotBeNull();
      list.Count().ShouldBeEqualTo(2);
      list.Last().CommenterName.ShouldBeEqualTo("A");
    }
  }
}