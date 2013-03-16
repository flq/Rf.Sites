using System;
using System.Text;
using DropNet.Models;
using NUnit.Framework;
using Rf.Sites.Frame.DropboxSupport;
using Rf.Sites.Test.Support;
using FluentAssertions;

namespace Rf.Sites.Test.DropboxSupport
{

    [TestFixture]
    public class ParsingOfMarkdownHeaders_Full_set
    {
        private MarkdownFile _file;

        [TestFixtureSetUp]
        public void Given()
        {
            _file = new MarkdownFile(new MetaData(), Encoding.Unicode.GetBytes(DataMother.MarkdownFullHeader()));
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