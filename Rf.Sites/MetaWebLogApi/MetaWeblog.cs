using System;
using System.Linq;
using CookComputing.XmlRpc;
using Rf.Sites.Actions;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using StructureMap;
using Environment = Rf.Sites.Frame.Environment;

namespace Rf.Sites.MetaWeblogApi
{
  /// <summary>
  /// Code used from the post
  /// http://nayyeri.net/implement-metaweblog-api-in-asp-net
  /// No licensing provided.
  /// </summary>
  [HandlerUrl(Url = "metaweblog.site")]
  public class MetaWeblog : XmlRpcService, IMetaWeblog
  {
    private readonly IContainer container;
    private readonly Environment environment;
    
    public MetaWeblog(Environment environment, IContainer container)
    {
      this.container = container;
      this.environment = environment;
    }

    string IMetaWeblog.AddPost(string blogid, string username, string password,
                               Post post, bool publish)
    {
      ensureRequestIntegrity(username, password);

      var converter = container.GetInstance<IObjectConverter<Post, Content>>();
      var content = converter.Convert(post);
      var repository = container.GetInstance<IRepository<Content>>();

      int id = 0;

      repository.Transacted(r=>id = r.Add(content));

      return id.ToString();
    }

    /// <summary>
    /// Current support: Only body and title is updated
    /// </summary>
    bool IMetaWeblog.UpdatePost(string postid, string username, string password,
                                Post post, bool publish)
    {
      ensureRequestIntegrity(username, password);

      var cRep = container.GetInstance<IRepository<Content>>();
      var content = cRep[Convert.ToInt32(postid)];
      content.Title = post.title;
      content.SetBody(post.description);
      
      cRep.Transacted(r=>r[content.Id] = content);

      return true;
    }

    CategoryInfo[] IMetaWeblog.GetCategories(string blogid, string username, string password)
    {
      ensureRequestIntegrity(username, password);
      var repTag = container.GetInstance<IRepository<Tag>>();

      var tags = (from t in repTag select new {t.Name, t.Description}).ToList();


      return (from t in tags
              let rssUrl = new Uri(
                environment.AbsoluteBaseUrl,
                FrameUtilities.RelativeUrlToAction<RssTagAction>(t.Name)).ToString()
              let webUrl = new Uri(
                environment.AbsoluteBaseUrl,
                FrameUtilities.RelativeUrlToAction<ContentTagAction>(t.Name)).ToString()
              select new CategoryInfo
                       {
                         categoryid = t.Name,
                         description = t.Description,
                         rssUrl = rssUrl,
                         htmlUrl = webUrl,
                         title = t.Name
                       }).ToArray();
    }

    Post IMetaWeblog.GetPost(string postid, string username, string password)
    {
      ensureRequestIntegrity(username, password);

      var rep = container.GetInstance<IRepository<Content>>();
      var c = rep[Convert.ToInt32(postid)];
      return container.GetInstance<IObjectConverter<Content, Post>>().Convert(c);
    }

    Post[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password,
                                      int numberOfPosts)
    {
      ensureRequestIntegrity(username, password);

      var rep = container.GetInstance<IRepository<Content>>();
      var converter = container.GetInstance<IObjectConverter<Content, Post>>();

      return (from c in rep orderby c.Created descending select c)
        .Take(numberOfPosts)
        .ToList() // Do the Query
        .Select(c => converter.Convert(c))
        .ToArray();
    }

    MediaObjectInfo IMetaWeblog.NewMediaObject(string blogid, string username, string password,
                                               MediaObject mediaObject)
    {
      ensureRequestIntegrity(username, password);

      var storage = container.GetInstance<IMediaStorage>();
      try
      {
        var relativeUrl = storage.StoreMedia(mediaObject.name, mediaObject.bits);
        return new MediaObjectInfo
                 {
                   url = new Uri(environment.AbsoluteBaseUrl, relativeUrl).ToString()
                 };
      }
      catch (Exception x)
      {
        throw new XmlRpcFaultException(2, string.Format("{0}:{1}", x.GetType().Name, x.Message));
      }
    }

    bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
    {
      ensureRequestIntegrity(username, password);
      throw new XmlRpcFaultException(1, "Deleting Post is currently not supported");
    }

    BlogInfo[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
    {
      ensureRequestIntegrity(username, password);

      var infoList = new []
                       {
                         new BlogInfo
                           {
                             blogid = "1",
                             blogName = environment.SiteTitle,
                             url = environment.AbsoluteBaseUrl.ToString()
                           }
                       };
      return infoList;
    }

    UserInfo IMetaWeblog.GetUserInfo(string key, string username, string password)
    {
      ensureRequestIntegrity(username, password);

      UserInfo info = new UserInfo
                        {
                          email = environment.SiteMasterEmail,
                          firstname = environment.SiteMasterName,
                          lastname = "",
                          nickname = "",
                          url = environment.AbsoluteBaseUrl.ToString(),
                          userid = "1"
                        };
      return info;
    }

    private void ensureRequestIntegrity(string username, string password)
    {
      if (username == environment.SiteMasterEmail && password == environment.SiteMasterPassword)
        return;

      throw new XmlRpcFaultException(0, "Usr Pwd Combination Fail");
    }
  }
}