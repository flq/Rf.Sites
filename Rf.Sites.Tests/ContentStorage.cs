using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rf.Sites.Domain;
using Rf.Sites.Tests.Frame;

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

    [Test]
    public void CanSaveAttachments()
    {
      object id;
      var c = maker.CreateContent();
      var attachment = new Attachment
                         {
                           Name = "Attached", Path = @"files/x.xml", Size = 1024,
                           Created = DateTime.Now
                         };

      using (var tx = Session.BeginTransaction())
      {
        c.AddAttachment(attachment);
        id = Session.Save(c);
        tx.Commit();
      }

      var cDb = Session.Load<Content>(id);
      cDb.ShouldNotBeNull();
      cDb.AttachmentCount.ShouldBeEqualTo(1);
      cDb.Attachments.ShouldNotBeNull();
      cDb.Attachments[0].Name.ShouldBeEqualTo("Attached");
    }

    [Test]
    public void CommentDefaultIsNotFromSiteMaster()
    {
      object id;
      var cmt = maker.CreateComment();
      using (var tx = Session.BeginTransaction())
      {
        id = Session.Save(cmt);
        tx.Commit();
      }
      Session.Clear();
      var cmtDb = Session.Load<Comment>(id);
      cmtDb.ShouldNotBeNull();
      cmtDb.IsFromSiteMaster.ShouldBeFalse();
    }
  }
}