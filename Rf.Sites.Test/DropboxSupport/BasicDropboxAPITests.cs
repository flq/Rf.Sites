using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using DropNet;
using DropNet.Exceptions;
using NUnit.Framework;

namespace Rf.Sites.Test.DropboxSupport
{

    [TestFixture, Category("Manual")]
    public class BasicDropboxAPITests
    {
        private const string DropboxapikeysTxt = @"..\..\dropboxapikeys.txt";

        [TestFixtureSetUp]
        public void Given()
        {
            if (!File.Exists(DropboxapikeysTxt))
                throw new ArgumentException("Need a file containing apiKey,appSecret,userToken,userSecret - obviously not checked in. Create one yourself and get on with life :)");
            var tokens = File.ReadAllText(DropboxapikeysTxt).Split(',');
            ApiKey = tokens[0];
            AppSecret = tokens[1];
            UserToken = tokens[2];
            UserSecret = tokens[3];
        }

        private string ApiKey { get; set; }
        private string AppSecret { get; set; }
        private string UserToken { get; set; }
        private string UserSecret { get; set; }

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
            var dropnet = FullyAuthorizedBitch();
            var info = dropnet.AccountInfo();
            Debug.WriteLine(info.email);
        }

        [Test]
        public void get_metadata()
        {
            var dropnet = FullyAuthorizedBitch();
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
            var dropnet = FullyAuthorizedBitch();
            var filecontent = Encoding.Unicode.GetString(dropnet.GetFile("/test.txt"));
            Debug.WriteLine(filecontent);
        }

        [Test]
        public void upload_file()
        {
            var dropnet = FullyAuthorizedBitch();
            dropnet.UploadFile("/", Path.GetRandomFileName() + ".txt", Encoding.Unicode.GetBytes("test. A real test."));
        }

        private DropNetClient FullyAuthorizedBitch()
        {
            var dropnet = new DropNetClient(ApiKey, AppSecret, UserToken, UserSecret)
            {
                UseSandbox = true
            };
            return dropnet;
        }
    }

}