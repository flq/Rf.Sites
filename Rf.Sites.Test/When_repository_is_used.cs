using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Frame;
using Rf.Sites.Test.Frame;

namespace Rf.Sites.Test
{
    [TestFixture]
    public class WhenTheRepositoryIsUsed : DbTestContext
    {
        [Test]
        public void CanObtainTheCountThroughTheRepository()
        {
            SaveAndCommit(Maker.CreateContent());
            
            Session.Clear();

            var count = Session.CreateQuery("select count(*) from Content").UniqueResult();
            
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
            var id = SaveAndCommit(Maker.CreateContent());
            var repC = new Repository<Content>(Factory);
            repC.Should().Contain(new Content(id));
        }

        [Test]
        public void BatchSizeEnsuresAllTagsAreLoaded()
        {
            var c = Maker.CreateContent();
            var tag1 = Maker.CreateTag();
            var tag2 = Maker.CreateTag();
            c.AssociateWithTag(tag1);
            c.AssociateWithTag(tag2);

            var id = SaveAndCommit(tag1, tag2, c);

            Session.Clear();

            var content = CreateRepository<Content>()[id];

            content.Tags[0].Name.Should().NotBeNull();
            content.Tags[1].Name.Should().NotBeNull();
        }

        [Test]
        public void CanQueryTheRepository()
        {
            var c = Maker.CreateContent();
            var tag1 = Maker.CreateTag("Tag1");
            c.AssociateWithTag(tag1);

            var c2 = Maker.CreateContent();
            c2.Title = "Foo";
            var tag2 = Maker.CreateTag("Tag2");
            c2.AssociateWithTag(tag2);

            using (var t = Session.BeginTransaction())
            {
                Session.Save(tag1);
                Session.Save(c);
                Session.Save(tag2);
                Session.Save(c2);
                t.Commit();
            }

            var repC = new Repository<Content>(Factory);

            var result =
              (from content in repC
               where content.Tags.Any(t => t.Name == "Tag2")
               select new { content.Id, content.Title }).ToArray();

            result.Should().HaveCount(1);
            result[0].Title.Should().Be("Foo");
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

            var result = (from c in repC
                          where c.Created >= new DateTime(2007, 1, 1) && c.Created <= new DateTime(2007, 12, 31)
                          select new { c.Title }).ToArray();

            result.Should().HaveCount(1);
            result[0].Title.Should().Be("Foo");
        }

        [Test]
        public void CanDoInStatementWithLinq()
        {
            Maker.ResetTagCount();
            var repT = new Repository<Tag>(Factory);
            repT.Transacted(r =>
                                {
                                    r.Add(Maker.CreateTag());
                                    r.Add(Maker.CreateTag());
                                    r.Add(Maker.CreateTag());
                                });
            Session.Clear();

            var tags = new[] { "Tag0", "Tag1" };
            var t2 = (from t in repT where tags.Contains(t.Name) select t.Name)
              .Distinct()
              .ToList();
            t2.Should().NotBeNull();
            t2.Should().HaveCount(2);
            t2.Should().NotContain(t => t == "Tag2");
        }

        [Test]
        public void AutomatedCommitInTransactionMethod()
        {
            var repC = new Repository<Content>(Factory);
            var cn = Maker.CreateContent();
            int id = -1;
            repC.Transacted(r => id = r.Add(cn));
            Session.Clear();

            var cn2 = repC[id];
            cn2.Should().NotBeNull();
            cn2.Title.Should().Be(cn.Title);
        }

        [Test]
        public void TransactedActionWillRollback()
        {
            var repC = new Repository<Content>(Factory);

            var currentContentCount = repC.Count;

            var cn = Maker.CreateContent();
            var cn2 = Maker.CreateContent();

            // ReSharper disable EmptyGeneralCatchClause
            // Part of the test
            try
            {
                repC.Transacted(r =>
                                  {
                                      r.Add(cn);
                                      r.Add(cn2);
                                      throw new Exception("Ouch");
                                  });
            }

            catch { }
            // ReSharper restore EmptyGeneralCatchClause

            Session.Clear();

            repC.Count.Should().Be(currentContentCount);
        }

        [Test]
        public void UpdateIsPossibleViaIdSetter()
        {
            var repC = new Repository<Content>(Factory);
            var cn = Maker.CreateContent();
            int id = 0;
            repC.Transacted(r => id = r.Add(cn));

            Session.Clear();
            var cn2 = repC[id];
            cn2.Title = "TheFoo";
            repC.Transacted(r => r[id] = cn2);

            Session.Clear();
            var cn3 = repC[id];
            cn3.Title.Should().Be("TheFoo");
        }
    }
}