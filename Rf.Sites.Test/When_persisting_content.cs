using System;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Test.Frame;
using FluentAssertions;

namespace Rf.Sites.Test
{
    [TestFixture]
    public class WhenPersistingContent : DbTestContext
    {
        [Test]
        public void Content_can_be_saved()
        {
            object id;

            using (var tx = Session.BeginTransaction())
            {
                var c = Maker.CreateContent();
                id = Session.Save(c);
                tx.Commit();
            }
            Session.Clear();

            using (var tx = Session.BeginTransaction())
            {
                var c1 = Session.Load<Content>(id);
                c1.Title.Should().Be("Title");
                tx.Commit();
            }

        }

        [Test]
        public void Content_can_be_tagged()
        {
            object tagId;
            var t = Maker.CreateTag();
            using (var tx = Session.BeginTransaction())
            {
                tagId = Session.Save(t);
                tx.Commit();
            }

            using (var tx = Session.BeginTransaction())
            {
                var c = Maker.CreateContent();
                c.AssociateWithTag(t);
                Session.Save(c);
                tx.Commit();
            }

            var tDb = Session.Load<Tag>(tagId);
            Assert.IsNotNull(tDb);

            tDb.RelatedContent.Should().HaveCount(1);
            tDb.RelatedContent[0].Title.Should().Be("Title");
        }

        [Test]
        public void Can_save_attachments()
        {
            object id;
            var c = Maker.CreateContent();
            var attachment = new Attachment
                               {
                                   Name = "Attached",
                                   Path = @"files/x.xml",
                                   Size = 1024,
                                   Created = DateTime.Now
                               };

            using (var tx = Session.BeginTransaction())
            {
                c.AddAttachment(attachment);
                id = Session.Save(c);
                tx.Commit();
            }

            var cDb = Session.Load<Content>(id);

            cDb.Should().NotBeNull();
            cDb.AttachmentCount.Should().Be(1);
            cDb.Attachments.Should().NotBeNull();
            cDb.Attachments[0].Name.Should().Be("Attached");
        }
    }
}