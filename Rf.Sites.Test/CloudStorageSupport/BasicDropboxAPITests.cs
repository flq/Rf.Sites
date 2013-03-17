using System.Diagnostics;
using System.IO;
using System.Text;
using DropNet;
using DropNet.Exceptions;
using NUnit.Framework;

namespace Rf.Sites.Test.CloudStorageSupport
{
    [TestFixture, Category("Manual")]
    public class BasicDropboxAPITests : DropboxAPIIntegrationContext
    {
        [Test]
        public void fire_up_the_client()
        {
            var dropnet = new DropNetClient(ApiKey, AppSecret);
            var uselogin = dropnet.GetToken();
            Debug.WriteLine("Token: " + uselogin.Token);
            Debug.WriteLine("Secret: " + uselogin.Secret);
            var url = dropnet.BuildAuthorizeUrl(uselogin);
            Debug.WriteLine("Url: " + url);
        }

        [Test]
        public void account_info()
        {
            var dropnet = GetAuthorizedClient();
            var info = dropnet.AccountInfo();
            Debug.WriteLine(info.email);
        }

        [Test]
        public void get_metadata()
        {
            var dropnet = GetAuthorizedClient();
            try
            {
                var metadata = dropnet.GetMetaData();
                Debug.WriteLine(metadata.Name);
                Debug.WriteLine(metadata.Path);
                Debug.WriteLine("Count: " + metadata.Contents.Count);
                foreach (var md in metadata.Contents)
                {
                    Debug.WriteLine(md.Name);
                }

            }
            catch (DropboxException x)
            {
                Debug.WriteLine(x.Response.Content);
            }
        }

        [Test]
        public void get_file()
        {
            var dropnet = GetAuthorizedClient();
            var filecontent = Encoding.Unicode.GetString(dropnet.GetFile("/test.txt"));
            Debug.WriteLine(filecontent);
        }

        [Test]
        public void upload_file()
        {
            var dropnet = GetAuthorizedClient();
            dropnet.UploadFile("/", Path.GetRandomFileName() + ".txt", Encoding.Unicode.GetBytes("test. A real test."));
        }
    }

}