using CookComputing.XmlRpc;
using NUnit.Framework;
using Rf.Sites.MetaWeblogApi;
using Rf.Sites.Tests.Frame;
using System.Linq;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class UsingTheMetaWeblogService
  {
    readonly MetaWeblogEnv mwl = new MetaWeblogEnv();

    [Test]
    [ExpectedException(typeof(XmlRpcFaultException))]
    public void UserAuthenticationFailsWhenWrongPasswordIsPassed()
    {
      var api = mwl.GetApi();
      api.AddPost("", mwl.Uid, "", new Post(), false);
    }

    public void UserAuthenticationIntegrityPasses()
    {
      var api = mwl.GetApi();
      api.AddPost("", mwl.Uid, mwl.Pwd, new Post(), false);
    }

    [Test]
    public void UserInfoProvidesSiteTitleAndUrl()
    {
      var api = mwl.GetApi();
      var ui = api.GetUserInfo("1", mwl.Uid, mwl.Pwd);
      ui.email.ShouldBeEqualTo(mwl.Uid);
    }

    [Test]
    public void BlogInfoProvidesSiteTitleAndUrl()
    {
      var api = mwl.GetApi();
      var blogInfos = api.GetUsersBlogs("1", mwl.Uid, mwl.Pwd);
      blogInfos.ShouldHaveLength(1);
      blogInfos[0].url.ShouldBeEqualTo(mwl.Url);
    }

    [Test]
    public void CategoriesRetrievesAvailableTags()
    {
      var api = mwl.GetApi();
      var cats = api.GetCategories("1", mwl.Uid, mwl.Pwd);
      cats.ShouldHaveLength(mwl.Tags.Count);
      cats[0].categoryid.ShouldBeEqualTo(mwl.Tags.First().Name);
    }
  }
}