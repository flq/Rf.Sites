using System;
using Rf.Sites.Frame;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Actions.CommentPost
{
  public class AttributeCommentToSiteMaster : IVmExtender<CommentUpdatePreparer>
  {
    private readonly Environment env;

    public AttributeCommentToSiteMaster(Environment env)
    {
      this.env = env;
    }

    public void Inspect(CommentUpdatePreparer commentInfo)
    {
      var c = commentInfo.Comment.Body;
      if (string.IsNullOrEmpty(c))
        return;
      var pwd = env.SiteMasterPassword;
      if (c.EndsWith("/" + pwd))
      {
        var comment = commentInfo.Comment;
        comment.IsFromSiteMaster = true;
        comment.CommenterEmail = env.SiteMasterEmail;
        comment.CommenterWebsite = env.SiteMasterWebPage;
        comment.Body = c.Replace("/" + pwd, "");
      }
    }
  }
}