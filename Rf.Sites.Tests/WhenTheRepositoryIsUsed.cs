using System;
using System.Linq;
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
        var c = Maker.CreateContent();
        Session.Save(c);
        t.Commit();
      }

      Session.Clear();

      object count = Session.CreateQuery("select count(*) from Content c").UniqueResult();

      var repC = new Repository<Content>(Factory);
      Assert.AreEqual(count, repC.Count);
    }

    [Test]
    public void ContainsWillSaythatItemDoesNotExist()
    {
      var c = new Content(13);
      var repC = new Repository<Content>(Factory);
      Assert.IsFalse(repC.Contains(c));
    }

    [Test]
    public void ContainsWillSaythatItemDoesExist()
    {
      object id;
      using (var t = Session.BeginTransaction())
      {
        var c = Maker.CreateContent();
        id = Session.Save(c);
        t.Commit();
      }
      var repC = new Repository<Content>(Factory);
      Assert.IsTrue(repC.Contains(new Content((int)id)));
    }

    [Test]
    public void BatchSizeEnsuresAllTagsAreLoaded()
    {
      object id;
      using (var t = Session.BeginTransaction())
      {
        var c = Maker.CreateContent();
        var tag1 = Maker.CreateTag();
        var tag2 = Maker.CreateTag();
        c.AssociateWithTag(tag1);
        c.AssociateWithTag(tag2);
        Session.Save(tag1);
        Session.Save(tag2);
        id = Session.Save(c);
        t.Commit();
      }

      Session.Clear();

      var repC = new Repository<Content>(Factory);

      var content = repC[(int) id];

      
      Assert.That(content.Tags[0].Name, Is.Not.Null);
      Console.WriteLine("Tag 1");
      Assert.That(content.Tags[1].Name, Is.Not.Null);
      Console.WriteLine("Tag 2");
    }

    [Test]
    public void CanQueryTheRepository()
    {
      var c = Maker.CreateContent();
      c.AddComment(Maker.CreateComment());
      var c2 = Maker.CreateContent();
      c2.AddComment(Maker.CreateComment("Foo"));
      c2.Title = "Foo";
      using (var t = Session.BeginTransaction())
      {
        Session.Save(c);
        Session.Save(c2);
        t.Commit();
      }

      var repC = new Repository<Content>(Factory);

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
      var c = Maker.CreateContent();

      using (var t = Session.BeginTransaction())
      {
        id = Session.Save(c);
        t.Commit();
      }

      Session.Clear();

      using (var t = Session.BeginTransaction())
      {
        var repC = new Repository<Content>(Factory);
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
      var cn = Maker.CreateContent();
      var repC = new Repository<Content>(Factory);
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
      var repC = new Repository<Content>(Factory);
      var cn = Maker.CreateContent();
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
      var repC = new Repository<Content>(Factory);
      var cn = Maker.CreateContent();
      cn.Created = new DateTime(2007, 01, 01);
      cn.Title = "Foo";
      var cn2 = Maker.CreateContent();
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

    [Test]
    public void CanDoInStatementWithLINQ()
    {
      Maker.ResetTagCount();
      var repT = new Repository<Tag>(Factory);
      using (var t = Session.BeginTransaction())
      {
        repT.Add(Maker.CreateTag());
        repT.Add(Maker.CreateTag());
        repT.Add(Maker.CreateTag());
        t.Commit();
      }

      Session.Clear();

      var tags = new[] {"Tag0", "Tag1"};
      var t2 = (from t in repT where tags.Contains(t.Name) select t.Name)
        .Distinct()
        .ToList();
      t2.ShouldNotBeNull();
      t2.ShouldHaveCount(2);
      t2.ShouldNotContain(t=>t == "Tag2");
    }

    [Test]
    public void AutomatedCommitInTransactionMethod()
    {
      var repC = new Repository<Content>(Factory);
      var cn = Maker.CreateContent();
      int id = -1;
      repC.Transacted(r=>id = r.Add(cn));
      Session.Clear();

      var cn2 = repC[id];
      cn2.ShouldNotBeNull();
      cn2.Title.ShouldBeEqualTo(cn.Title);
    }

    [Test]
    public void TransactedActionWillRollback()
    {
      var repC = new Repository<Content>(Factory);

      var currentContentCount = repC.Count;

      var cn = Maker.CreateContent();
      var cn2 = Maker.CreateContent();
      try
      {
        repC.Transacted(r=>
                          {
                            r.Add(cn);
                            r.Add(cn2);
                            throw new Exception("Ouch");
                          });
      }
      catch {}

      Session.Clear();

      repC.Count.ShouldBeEqualTo(currentContentCount);
    }

    [Test]
    public void UpdateIsPossibleViaIdSetter()
    {
      var repC = new Repository<Content>(Factory);
      var cn = Maker.CreateContent();
      int id = 0;
      repC.Transacted(r=>id = r.Add(cn));
      
      Session.Clear();
      var cn2 = repC[id];
      cn2.Title = "TheFoo";
      repC.Transacted(r=>r[id] = cn2);

      Session.Clear();
      var cn3 = repC[id];
      cn3.Title.ShouldBeEqualTo("TheFoo");
    }
  }
}