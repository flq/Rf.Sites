using CookComputing.XmlRpc;
using System.Collections.Generic;
using Rf.Sites.Frame;
using StructureMap;

namespace Rf.Sites.MetaWeblogApi
{
  /// <summary>
  /// Code used from the post
  /// http://nayyeri.net/implement-metaweblog-api-in-asp-net
  /// No licensing provided.
  /// As a Handler, the class gets constructed by the .Net HTTP Runtime.
  /// Hence, when the site is running the <see cref="ensureRequestIntegrity"/>
  /// method will ensure provision of dependencies directly through the ObjectFactory.
  /// Tests may use the non-default constructor to provide dependencies directly.
  /// </summary>
  public class MetaWeblog : XmlRpcService, IMetaWeblog
  {
    private bool initializedThroughNonDefaultConstructor;

    public MetaWeblog()
    {
      
    }

    public MetaWeblog(Environment environment)
    {
      Environment = environment;
      initializedThroughNonDefaultConstructor = true;
    }

    public Environment Environment
    {
      private get;
      set;
    }

    #region IMetaWeblog Members


    string IMetaWeblog.AddPost(string blogid, string username, string password,
                               Post post, bool publish)
    {
      ensureRequestIntegrity(username, password);

      string id = string.Empty;

      // TODO: Implement your own logic to add the post and set the id

      return id;
    }

    bool IMetaWeblog.UpdatePost(string postid, string username, string password,
                                Post post, bool publish)
    {
      ensureRequestIntegrity(username, password);

      bool result = false;

      // TODO: Implement your own logic to add the post and set the result

      return result;
    }

    Post IMetaWeblog.GetPost(string postid, string username, string password)
    {
      ensureRequestIntegrity(username, password);

      Post post = new Post();

      // TODO: Implement your own logic to update the post and set the post

      return post;
    }

    CategoryInfo[] IMetaWeblog.GetCategories(string blogid, string username, string password)
    {
      ensureRequestIntegrity(username, password);

      List<CategoryInfo> categoryInfos = new List<CategoryInfo>();

      // TODO: Implement your own logic to get category info and set the categoryInfos

      return categoryInfos.ToArray();
    }

    Post[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password,
                                      int numberOfPosts)
    {
      ensureRequestIntegrity(username, password);

      List<Post> posts = new List<Post>();

      // TODO: Implement your own logic to get posts and set the posts

      return posts.ToArray();
    }

    MediaObjectInfo IMetaWeblog.NewMediaObject(string blogid, string username, string password,
                                               MediaObject mediaObject)
    {
      ensureRequestIntegrity(username, password);

      MediaObjectInfo objectInfo = new MediaObjectInfo();

      // TODO: Implement your own logic to add media object and set the objectInfo

      return objectInfo;

    }

    bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
    {
      ensureRequestIntegrity(username, password);

      bool result = false;

      // TODO: Implement your own logic to delete the post and set the result

      return result;
    }

    BlogInfo[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
    {
      ensureRequestIntegrity(username, password);

      var infoList = new []
                       {
                         new BlogInfo
                           {
                             blogid = "1",
                             blogName = Environment.SiteTitle,
                             url = Environment.AbsoluteBaseUrl.ToString()
                           }
                       };
      return infoList;
    }

    UserInfo IMetaWeblog.GetUserInfo(string key, string username, string password)
    {
      ensureRequestIntegrity(username, password);

      UserInfo info = new UserInfo
                        {
                          email = Environment.SiteMasterEmail,
                          firstname = Environment.SiteMasterName,
                          lastname = "",
                          nickname = "",
                          url = Environment.AbsoluteBaseUrl.ToString(),
                          userid = "1"
                        };
      return info;
    }

    #endregion

    #region Private Methods

    private void ensureRequestIntegrity(string username, string password)
    {
      if (!initializedThroughNonDefaultConstructor)
        ObjectFactory.BuildUp(this);

      if (username == Environment.SiteMasterEmail && password == Environment.SiteMasterPassword)
        return;

      throw new XmlRpcFaultException(0, "Usr Pwd Combination Fail");
    }

    #endregion
  }
}