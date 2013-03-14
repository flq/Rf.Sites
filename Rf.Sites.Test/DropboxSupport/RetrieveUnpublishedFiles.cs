using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using Rf.Sites.Frame.DropboxSupport;

namespace Rf.Sites.Test.DropboxSupport
{

    [TestFixture,Category("Manual")]
    public class RetrieveUnpublishedFiles : DropboxAPIIntegrationContext
    {
        private string _randomFile1;
        private string _randomFile2;
        private string _publishedFile;

        protected override void AdditionalSetup()
        {
            _randomFile1 = Path.GetRandomFileName() + ".md";
            _randomFile2 = Path.GetRandomFileName() + ".md";
            _publishedFile = Path.GetRandomFileName() + DropboxFacade.PublishedMarker + ".md";

            var client = GetAuthorizedClient();
            client.UploadFile("/", _randomFile1, GetFileContent());
            client.UploadFile("/", _randomFile2, GetFileContent());
            client.UploadFile("/", _publishedFile, GetFileContent());
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
            var c = GetAuthorizedClient();
            c.Delete("/" + _randomFile1);
            c.Delete("/" + _randomFile2);
            c.Delete("/" + _publishedFile);
        }

        [Test]
        public void the_two_unpublished_files_are_obtained()
        {
            var f = GetDropboxFacade();
            var files = f.GetAllUnpublished();
            files.Should().HaveCount(2);
            Guid guid;

            files[0].Name.Should().BeOneOf(_randomFile1, _randomFile2);
            Guid.TryParse(files[0].RawContents, out guid).Should().BeTrue("file contents 1 was a guid");

            files[1].Name.Should().BeOneOf(_randomFile1, _randomFile2);
            Guid.TryParse(files[1].RawContents, out guid).Should().BeTrue("file contents 2 was a guid");
        }
    }

}