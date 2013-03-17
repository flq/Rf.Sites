using System.IO;
using System.Linq;
using DropNet.Exceptions;
using DropNet.Models;
using FluentAssertions;
using NUnit.Framework;
using Rf.Sites.Frame.CloudStorageSupport;

namespace Rf.Sites.Test.CloudStorageSupport
{
    [TestFixture,Category("Manual")]
    public class RenameOfPublishedFiles : DropboxAPIIntegrationContext
    {
        private string _randomFile1;
        private ICloudStorageFacade _f;
        private MarkdownFile _file;
        private MetaData _meta;

        protected override void AdditionalSetup()
        {
            _randomFile1 = Path.GetRandomFileName() + ".md";
            var client = GetAuthorizedClient();
            client.UploadFile("/", _randomFile1, GetFileContent());

            _f = GetDropboxFacade();
            var files = _f.GetAllUnpublished();
            _file = files[0];
            _f.UpdatePublishState(files);

            var c = GetAuthorizedClient();
            _meta = c.GetMetaData();
        }

        [Test]
        public void metadata_shows_published_file()
        {
            var m = _meta.Contents.FirstOrDefault(md => md.Name.StartsWith(Path.GetFileNameWithoutExtension(_randomFile1)));
            m.Should().NotBeNull();
            m.Name.Should().Contain(DropboxFacade.PublishedMarker);
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
            var c = GetAuthorizedClient();
            try
            {
                c.Delete("/" + _file.Name);
            }
            catch (DropboxException)
            {
                // The file should actually not exist anymore, that raises an exception
            }
            c.Delete("/" + _file.PublishedName);

        }
    }
}