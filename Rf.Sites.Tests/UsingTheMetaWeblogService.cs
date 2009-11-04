using System;
using CookComputing.XmlRpc;
using NUnit.Framework;
using Rf.Sites.MetaWeblogApi;
using Environment=Rf.Sites.Frame.Environment;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class UsingTheMetaWeblogService
  {
    private Environment env = new Environment
                                {
                                  SiteMasterEmail = uid,
                                  SiteMasterPassword = pwd,
                                  SiteMasterWebPage = "http://lugos.com",
                                  AbsoluteBaseUrl = new Uri(url)
                                };

    private const string url = "http://thesite.com/";
    private const string uid = "a@b.com";
    private const string pwd = "pwd";

    [Test]
    [ExpectedException(typeof(XmlRpcFaultException))]
    public void UserAuthenticationFailsWhenWrongPasswordIsPassed()
    {
      IMetaWeblog mwl = new MetaWeblog(env);
      mwl.AddPost("", uid, "", new Post(), false);
    }

    public void UserAuthenticationIntegrityPasses()
    {
      IMetaWeblog mwl = new MetaWeblog(env);
      mwl.AddPost("", uid, pwd, new Post(), false);
    }

    [Test]
    public void UserInfoProvidesSiteTitleAndUrl()
    {
      IMetaWeblog mwl = new MetaWeblog(env);
      var ui = mwl.GetUserInfo("ignored", uid, pwd);
      ui.email.ShouldBeEqualTo(env.SiteMasterEmail);
    }

    [Test]
    public void BlogInfoProvidesSiteTitleAndUrl()
    {
      IMetaWeblog mwl = new MetaWeblog(env);
      var blogInfos = mwl.GetUsersBlogs("ignored", uid, pwd);
      blogInfos.ShouldHaveLength(1);
      blogInfos[0].url.ShouldBeEqualTo(url);
    }
  }
}