using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Moq;
using NUnit.Framework;
using Rf.Sites.Actions;
using Rf.Sites.Actions.Args;
using Rf.Sites.Actions.TagCloud;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Tests.DataScenarios;
using Rf.Sites.Tests.Frame;
using StructureMap;
using Moq.Protected;
using Environment=Rf.Sites.Frame.Environment;

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

      var cnt = new Container(
        cn =>
          {
            cn.ForRequestedType<IObjectConverter<Comment, CommentVM>>()
              .TheDefault.IsThis(ObjectConverter.From((Comment c) => new CommentVM(c, null)));
            cn.ForRequestedType<IObjectConverter<Attachment, AttachmentVM>>()
              .TheDefault.IsThis(ObjectConverter.From((Attachment a) => new AttachmentVM(a,null)));
          });

      
      var action = new ContentEntryAction(ArgsFrom.Id(1), mock.Object) {Container = cnt};
      var model = action.Execute().GetModelFromAction<ContentViewModel>();
      
      model.Title.ShouldBeEqualTo("Foo");
      mock.Verify();
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
    public void TagcloudActionWorksAsIntended()
    {
      ActionEnv env = new ActionEnv();
      env.UseInMemoryDb();
      env.DataScenario<AFewTagsAndNumerousContent>();

      var a = new TagcloudIndexAction();
      env.GetAction(a);

      var tl = a.Execute().GetModelFromAction<TagList>();
      tl.ShouldNotBeNull();

    }
  }
}