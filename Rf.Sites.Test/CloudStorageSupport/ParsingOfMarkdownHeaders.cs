using System;
using System.Text;
using NUnit.Framework;
using Rf.Sites.Frame.CloudStorageSupport;
using Rf.Sites.Test.Support;
using FluentAssertions;

namespace Rf.Sites.Test.CloudStorageSupport
{

    [TestFixture]
    public class ParsingOfMarkdownHeaders_Full_set
    {
        private MarkdownFile _file;

        [TestFixtureSetUp]
        public void Given()
        {
            _file = new MarkdownFile("name", Encoding.UTF8.GetBytes(DataMother.MarkdownFullHeader()));
        }

        [Test]
        public void is_valid()
        {
            _file.IsValid.Should().BeTrue();
        }

        [Test]
        public void Title_is_set()
        {
            _file.Title.Should().Be("A new post");
        }

        [Test]
        public void Publishdate_is_set()
        {
            _file.Publish.Should().Be(new DateTime(2013,3,20));
        }

        [Test]
        public void tags_available()
        {
            _file.Tags.Should().BeEquivalentTo(new [] {"programming", ".net"});
        }

        [Test]
        public void body_starts_correctly()
        {
            _file.PostBody.Should().StartWith("# Hello");
        }

    }

}