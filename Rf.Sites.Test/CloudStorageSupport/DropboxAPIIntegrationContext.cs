using System;
using System.IO;
using System.Text;
using DropNet;
using NUnit.Framework;
using Rf.Sites.Frame.CloudStorageSupport;

namespace Rf.Sites.Test.CloudStorageSupport
{
    public class DropboxAPIIntegrationContext
    {
        private const string DropboxapikeysTxt = @"..\..\dropboxapikeys.txt";
        protected string ApiKey { get; set; }
        protected string AppSecret { get; set; }
        private string UserToken { get; set; }
        private string UserSecret { get; set; }

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
            AdditionalSetup();
        }

        protected virtual void AdditionalSetup()
        {
            
        }

        protected ICloudStorageFacade GetDropboxFacade()
        {
            return new DropboxFacade(GetAuthorizedClient);
        }

        protected DropNetClient GetAuthorizedClient()
        {
            var dropnet = new DropNetClient(ApiKey, AppSecret, UserToken, UserSecret)
            {
                UseSandbox = true
            };
            return dropnet;
        }

        protected static byte[] GetFileContent()
        {
            return Encoding.Unicode.GetBytes(Guid.NewGuid().ToString());
        }
    }
}