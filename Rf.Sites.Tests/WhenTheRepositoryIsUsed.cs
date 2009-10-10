using System;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class WhenTheRepositoryIsUsed : DbTestBase
  {
    [Test]
    public void CanObtainTheCountThroughTheRepository()
    {
      using (var t = Session.BeginTransaction())
      {
        var c = maker.CreateContent();
        Session.Save(c);
        t.Commit();
      }

      var repC = new Repository<Content>(factory);
      Assert.AreEqual(1, repC.Count);
    }

    [Test]
    public void ContainsWillSaythatItemDoesNotExist()
    {
      var c = new Content(13);
      var repC = new Repository<Content>(factory);
      Assert.IsFalse(repC.Contains(c));
    }

    [Test]
    public void ContainsWillSaythatItemDoesExist()
    {
      object id;
      using (var t = Session.BeginTransaction())
      {
        var c = maker.CreateContent();
        id = Session.Save(c);
        t.Commit();
      }
      var repC = new Repository<Content>(factory);
      Assert.IsTrue(repC.Contains(new Content((int)id)));
    }

    [Test]
    public void BatchSizeEnsuresAllTagsAreLoaded()
    {
      object id;
      using (var t = Session.BeginTransaction())
      {
        var c = maker.CreateContent();
        var tag1 = maker.CreateTag();
        var tag2 = maker.CreateTag();
        c.AssociateWithTag(tag1);
        c.AssociateWithTag(tag2);
        Session.Save(tag1);
        Session.Save(tag2);
        id = Session.Save(c);
        t.Commit();
      }

      Session.Clear();

      var repC = new Repository<Content>(factory);

      var content = repC[(int) id];

      
      Assert.That(content.Tags[0].Name, Is.Not.Null);
      Console.WriteLine("Tag 1");
      Assert.That(content.Tags[1].Name, Is.Not.Null);
      Console.WriteLine("Tag 2");
    }

    [Test]
    public void CanQueryTheRepository()
    {
      var c = maker.CreateContent();
      c.AddComment(maker.CreateComment());
      var c2 = maker.CreateContent();
      c2.AddComment(maker.CreateComment("Foo"));
      c2.Title = "Foo";
      using (var t = Session.BeginTransaction())
      {
        Session.Save(c);
        Session.Save(c2);
        t.Commit();
      }

      var repC = new Repository<Content>(factory);

      var result =
        (from content in repC
         where content.Comments.Any(cn => cn.CommenterName == "Foo")
         select new { content.Id, content.Title }).ToArray();

      Assert.That(result, Has.Length(1));
      Assert.AreEqual("Foo", result[0].Title);
    }

    [Test]
    public void UpdatesAnItem()
    {
      object id;
      var c = maker.CreateContent();

      using (var t = Session.BeginTransaction())
      {
        id = Session.Save(c);
        t.Commit();
      }

      Session.Clear();

      using (var t = Session.BeginTransaction())
      {
        var repC = new Repository<Content>(factory);
        var c2 = repC[(int)id];
        c2.Title = "Bar";
        t.Commit();
      }

      Session.Clear();

      var c3 = Session.Get<Content>(id);

      Assert.AreEqual("Bar", c3.Title);
    }

    [Test]
    public void CanAddNew()
    {
      var cn = maker.CreateContent();
      var repC = new Repository<Content>(factory);
      using (var t = Session.BeginTransaction())
      {
        repC.Add(cn);
        t.Commit();
      }

      var c2 = repC.Select(c => c.Title == "Title").First();
      Assert.IsNotNull(c2);
    }

    [Test]
    public void IndexerForIDLoad()
    {
      var repC = new Repository<Content>(factory);
      var cn = maker.CreateContent();
      int id;
      using (var t = Session.BeginTransaction())
      {
        id = repC.Add(cn);
        t.Commit();
      }

      Session.Clear();

      var cn2 = repC[id];

      Assert.IsNotNull(cn2);
      Assert.AreEqual("Title", cn2.Title);
    }

    [Test]
    public void QueryByTimePartsIsDoneLikeThis()
    {
      var repC = new Repository<Content>(factory);
      var cn = maker.CreateContent();
      cn.Created = new DateTime(2007, 01, 01);
      cn.Title = "Foo";
      var cn2 = maker.CreateContent();
      cn2.Created = new DateTime(2008, 01, 01);

      using (var t = Session.BeginTransaction())
      {
        repC.Add(cn);
        repC.Add(cn2);
        t.Commit();
      }

      Session.Clear();

      var result = (from c in repC where c.Created >= new DateTime(2007,1,1) && c.Created <= new DateTime(2007, 12, 31) 
                    select new {c.Title}).ToArray();

      result.ShouldHaveLength(1);
      result[0].Title.ShouldBeEqualTo("Foo");
    }

  }
}