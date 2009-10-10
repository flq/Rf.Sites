using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Rf.Sites.Actions;
using Rf.Sites.Actions.Args;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Models;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class WhenActionsAreUsed : DbTestBase
  {
    [Test]
    public void ContentEntryReturnsLoadedContentInViewModel()
    {
      var mock = new Mock<IRepository<Content>>();
      mock.Setup(rep => rep[1]).Returns(new Content(1) {Title = "Foo"});

      var action = new ContentEntryAction(ArgsFrom.Id(1), mock.Object);
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

      var action = new ContentTagAction(ArgsFrom.Value("Foo"), rep);
      var model = action.Execute().GetModelFromAction<ContentFragments>();
      model.ShouldHaveCount(1);
      model.Title.ShouldBeEqualTo("Foo");
      model.CurrentPage.ShouldBeEqualTo(0);
      model[0].Title.ShouldBeEqualTo("Bar");
    }

    [Test]
    public void NHibernateCheckOfGettingTeasers()
    {
      var tag = maker.CreateTag();

      using (var t = Session.BeginTransaction())
      {
        var c = maker.CreateContent();  
        c.AssociateWithTag(tag);
        Session.Save(tag);
        Session.Save(c);
        t.Commit();
      }

      Session.Clear();
      var action = new ContentTagAction(ArgsFrom.Value(tag.Name), new Repository<Content>(factory));
      var model = action.Execute().GetModelFromAction<List<ContentFragmentViewModel>>();
      model.ShouldHaveCount(1);

    }

  }
}