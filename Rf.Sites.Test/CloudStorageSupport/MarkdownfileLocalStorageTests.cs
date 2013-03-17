using System.Text;
using DropNet.Models;
using NHibernate.Linq;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Frame.CloudStorageSupport;
using Rf.Sites.Test.Frame;
using Rf.Sites.Test.Support;
using System.Linq;
using FluentAssertions;

namespace Rf.Sites.Test.CloudStorageSupport
{
    [TestFixture]
    public class MarkdownfileLocalStorageTests : DbTestContext
    {
        private MarkdownFile _file;
        private StoreToDb _storeToDb;

        [TestFixtureSetUp]
        public void Given()
        {
            SaveAndCommit(new Tag {Name = "programming"}, new Tag {Name = ".NET"});

            _file = new MarkdownFile(new MetaData(), Encoding.Unicode.GetBytes(DataMother.MarkdownFullHeader()));
            _storeToDb = new StoreToDb(() => Session);
            _storeToDb.Store(_file);
        }

        [Test]
        public void content_has_the_correct_values()
        {
            var content = GetStoredContent();
            content.Should().NotBeNull();
            content.IsMarkdown.Should().BeTrue();
            content.Body.Should().StartWith("# Hello");
        }

        [Test]
        public void tags_have_been_associated()
        {
            var c = GetStoredContent();
            c.Tags.Should().HaveCount(2);
            c.Tags.Select(t => t.Name).Should().BeEquivalentTo(new[] {"programming", ".NET"});
        }

        [Test]
        public void file_is_marked_as_committed()
        {
            _file.HasBeenStoredLocally.Should().BeTrue();
        }

        private Content GetStoredContent()
        {
            return Session.Query<Content>().FirstOrDefault(c => c.Title == "A new post");
        }
    }
}