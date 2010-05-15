using System;
using CookComputing.XmlRpc;
using Moq;
using NUnit.Framework;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.MetaWeblogApi;
using Rf.Sites.Tests.Frame;
using System.Linq;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class UsingTheMetaWeblogService
  {
    readonly MetaWeblogEnv mwl = new MetaWeblogEnv();

    [TearDown]
    public void ReleaseResources()
    {
      mwl.ReleaseResources();
    }

    [Test]
    [ExpectedException(typeof(XmlRpcFaultException))]
    public void UserAuthenticationFailsWhenWrongPasswordIsPassed()
    {
      var api = mwl.GetApi();
      api.AddPost("", mwl.Uid, "", new Post(), false);
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
      var tagR = new TestRepository<Tag>
          {
            new Tag {Created = DateTime.Now, Name = "1", Description = "A"},
            new Tag {Created = DateTime.Now, Name = "2", Description = "B"}
          };

      mwl.OverloadContainer(c => c.For<IRepository<Tag>>().Use(tagR));
      var api = mwl.GetApi();
      var cats = api.GetCategories("1", mwl.Uid, mwl.Pwd);
      cats.ShouldHaveLength(tagR.Count);
      cats[0].categoryid.ShouldBeEqualTo(tagR.First().Name);
    }

    [Test]
    public void MediaStorageUsesTheCorrespondingComponent()
    {
      var mS = new Mock<IMediaStorage>();
      var content = new byte[] {1, 2, 3};
      mS.Setup(m => m.StoreMedia("test", content)).Returns("test");
      mwl.OverloadContainer(c => c.For<IMediaStorage>().Use(mS.Object));
      var api = mwl.GetApi();

      var result = api.NewMediaObject("1", mwl.Uid, mwl.Pwd, new MediaObject {name = "test", bits = content});

      mS.Verify();
      result.url.ShouldBeEqualTo(mwl.Url + "test");
    }

    [Test]
    public void GetPostViaMwlApi()
    {
      var cR = new TestRepository<Content>
                 {
                   new Content(10) {Title = "A", Body = "B"}
                 };
      cR[10].AssociateWithTag(new Tag { Name = "Foo"});
      cR[10].AssociateWithTag(new Tag { Name = "Bar" });

      mwl.OverloadContainer(c=>c.For<IRepository<Content>>().Use(cR));
      var api = mwl.GetApi();
      var p = api.GetPost("10", mwl.Uid, mwl.Pwd);
      p.postid.ShouldBeEqualTo(10);
      p.categories.ShouldHaveLength(2);
      p.title.ShouldBeEqualTo("A");
    }

    [Test]
    public void UpdatePostViaMwlApi()
    {
      var cR = new TestRepository<Content>
                 {
                   new Content(10) {Title = "A", Body = "B"}
                 };
      cR[10].AssociateWithTag(new Tag { Name = "Foo" });
      cR[10].AssociateWithTag(new Tag { Name = "Bar" });

      mwl.OverloadContainer(c => c.For<IRepository<Content>>().Use(cR));
      var api = mwl.GetApi();
      api.UpdatePost("10", mwl.Uid, mwl.Pwd, new Post { postid = 10, title = "Hi", description = "Ho"}, true);

      var content = cR[10];
      content.Title.ShouldBeEqualTo("Hi");
      content.Body.ShouldBeEqualTo("Ho");
      content.Teaser.ShouldBeEqualTo("Ho");
    }

    [Test]
    public void GetRecentPostsViaApi()
    {
      var theDate = DateTime.Now - TimeSpan.FromDays(10);

      var cR = new TestRepository<Content>();
      for (int i = 0; i < 5; i++)
        cR.Add(new Content(i) {Title = "A" + i, Created = theDate + TimeSpan.FromDays(i)});

      mwl.OverloadContainer(c => c.For<IRepository<Content>>().Use(cR));
      var api = mwl.GetApi();

      var posts = api.GetRecentPosts("1", mwl.Uid, mwl.Pwd, 4);
      posts.ShouldHaveLength(4);
      posts.ShouldNotContain(p=>p.title == "A0");
      posts.ShouldContain(p => p.title == "A4");
    }

    [Test]
    public void ThePostToContentConverterReEscapesLiveWriterPointlessEscaping()
    {
      var post2Content = new PostToContentConverter(new TestRepository<Tag>());
      var c = post2Content.Convert(new Post
                                     {
                                       title = "Some PowerShell Foo...playin&rsquo; with da Task Scheduler",
                                       description = "<p>needed body</p>"
                                     });
      c.Title.ShouldBeEqualTo("Some PowerShell Foo...playin’ with da Task Scheduler");
    }

  }
}