using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rf.Sites.Domain;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class ContentStorage : DbTestBase
  {
    [Test]
    public void SaveContent()
    {
      object id;
      
      using (var tx = Session.BeginTransaction()) {
        var c = maker.CreateContent();
        id = Session.Save(c);
        tx.Commit();
      }
      Session.Clear();

      using (var tx = Session.BeginTransaction())
      {
        var c1 = Session.Load<Content>(id);
        Assert.AreEqual("Title", c1.Title);
        tx.Commit();
      }

    }

    [Test]
    public void ContentIsTagged()
    {
      object tagId;
      var t = maker.CreateTag();
      using (var tx = Session.BeginTransaction())
      {
        tagId = Session.Save(t);
        tx.Commit();
      }

      using (var tx = Session.BeginTransaction())
      {
        var c = maker.CreateContent();
        c.AssociateWithTag(t);
        Session.Save(c);
        tx.Commit();
      }

      var tDb = Session.Load<Tag>(tagId);
      Assert.IsNotNull(tDb);
      Assert.That(tDb.RelatedContent, Has.Count(1));
      Assert.AreEqual("Title", tDb.RelatedContent[0].Title);

    }

    [Test]
    public void CommentsCanBeAdded()
    {
      object id;

      var c = maker.CreateContent();
      var comment = maker.CreateComment();

      using (var tx = Session.BeginTransaction())
      {
        c.AddComment(comment);
        id = Session.Save(c);
        tx.Commit();
      }


      var cDb = Session.Load<Content>(id);
      Assert.IsNotNull(cDb);
      Assert.That(cDb.Comments, Has.Count(1));
      Assert.AreEqual(comment.CommenterName, cDb.Comments[0].CommenterName);

    }
  }
}